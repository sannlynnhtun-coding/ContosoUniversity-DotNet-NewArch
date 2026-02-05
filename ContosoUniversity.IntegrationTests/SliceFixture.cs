using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using ContosoUniversity.Domain.Infrastructure;
using ContosoUniversity.Domain.Shared;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Networks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using Testcontainers.MsSql;
using Xunit;

namespace ContosoUniversity.IntegrationTests;

[CollectionDefinition(nameof(SliceFixture))]
public class SliceFixtureCollection : ICollectionFixture<SliceFixture> { }

public class SliceFixture : IAsyncLifetime
{
    private Respawner _respawner;
    private IServiceScopeFactory _scopeFactory;
    private WebApplicationFactory<Program> _factory;
    private string _connectionString;

    class ContosoTestApplicationFactory 
        : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((_, configBuilder) =>
            {
                configBuilder.AddInMemoryCollection(new Dictionary<string, string>
                {
                    {"ConnectionStrings:db", ConnectionString}
                });
            });
        }

        public required string ConnectionString { get; init; }
    }

    public async Task ExecuteScopeAsync(Func<IServiceProvider, Task> action)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<SchoolContext>();

        try
        {
            await dbContext.BeginTransactionAsync();

            await action(scope.ServiceProvider);

            await dbContext.CommitTransactionAsync();
        }
        catch (Exception)
        {
            dbContext.RollbackTransaction(); 
            throw;
        }
    }

    public async Task<T> ExecuteScopeAsync<T>(Func<IServiceProvider, Task<T>> action)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<SchoolContext>();

        try
        {
            await dbContext.BeginTransactionAsync();

            var result = await action(scope.ServiceProvider);

            await dbContext.CommitTransactionAsync();

            return result;
        }
        catch (Exception)
        {
            dbContext.RollbackTransaction();
            throw;
        }
    }

    public Task ExecuteDbContextAsync(Func<SchoolContext, Task> action) 
        => ExecuteScopeAsync(sp => action(sp.GetService<SchoolContext>()));

    public Task ExecuteDbContextAsync(Func<SchoolContext, ValueTask> action) 
        => ExecuteScopeAsync(sp => action(sp.GetService<SchoolContext>()).AsTask());

    public Task<T> ExecuteDbContextAsync<T>(Func<SchoolContext, Task<T>> action) 
        => ExecuteScopeAsync(sp => action(sp.GetService<SchoolContext>()));

    public Task<T> ExecuteDbContextAsync<T>(Func<SchoolContext, ValueTask<T>> action) 
        => ExecuteScopeAsync(sp => action(sp.GetService<SchoolContext>()).AsTask());

    public Task ExecuteServiceAsync<TService>(Func<TService, Task> action)
        => ExecuteScopeAsync(sp => action(sp.GetRequiredService<TService>()));

    public Task<TResult> ExecuteServiceAsync<TService, TResult>(Func<TService, Task<TResult>> action)
        => ExecuteScopeAsync(sp => action(sp.GetRequiredService<TService>()));

    public Task InsertAsync<T>(params T[] entities) where T : class
    {
        return ExecuteDbContextAsync(db =>
        {
            foreach (var entity in entities)
            {
                db.Set<T>().Add(entity);
            }
            return db.SaveChangesAsync();
        });
    }

    public Task InsertAsync<TEntity>(TEntity entity) where TEntity : class
    {
        return ExecuteDbContextAsync(db =>
        {
            db.Set<TEntity>().Add(entity);

            return db.SaveChangesAsync();
        });
    }

    public Task InsertAsync<TEntity, TEntity2>(TEntity entity, TEntity2 entity2) 
        where TEntity : class
        where TEntity2 : class
    {
        return ExecuteDbContextAsync(db =>
        {
            db.Set<TEntity>().Add(entity);
            db.Set<TEntity2>().Add(entity2);

            return db.SaveChangesAsync();
        });
    }

    public Task InsertAsync<TEntity, TEntity2, TEntity3>(TEntity entity, TEntity2 entity2, TEntity3 entity3) 
        where TEntity : class
        where TEntity2 : class
        where TEntity3 : class
    {
        return ExecuteDbContextAsync(db =>
        {
            db.Set<TEntity>().Add(entity);
            db.Set<TEntity2>().Add(entity2);
            db.Set<TEntity3>().Add(entity3);

            return db.SaveChangesAsync();
        });
    }

    public Task InsertAsync<TEntity, TEntity2, TEntity3, TEntity4>(TEntity entity, TEntity2 entity2, TEntity3 entity3, TEntity4 entity4) 
        where TEntity : class
        where TEntity2 : class
        where TEntity3 : class
        where TEntity4 : class
    {
        return ExecuteDbContextAsync(db =>
        {
            db.Set<TEntity>().Add(entity);
            db.Set<TEntity2>().Add(entity2);
            db.Set<TEntity3>().Add(entity3);
            db.Set<TEntity4>().Add(entity4);

            return db.SaveChangesAsync();
        });
    }

    public Task<T> FindAsync<T>(int id)
        where T : class, IEntity
    {
        return ExecuteDbContextAsync(db => db.Set<T>().FindAsync(id).AsTask());
    }

    private int _courseNumber;
    private int _courseNumberBase = 1;
    private MsSqlContainer _msSqlContainer;
    private INetwork _network;

    /// <summary>
    /// Call from test class InitializeAsync to use a unique range of course IDs per test class (avoids parallel collision).
    /// </summary>
    public void SetCourseNumberBase(int baseValue)
    {
        _courseNumberBase = baseValue;
        _courseNumber = 0;
    }

    public int NextCourseNumber() => _courseNumberBase + Interlocked.Increment(ref _courseNumber);

    public async Task InitializeAsync()
    {
        string connectionString;

        try
        {
            connectionString = await InitializeWithDockerAsync();
        }
        catch (DotNet.Testcontainers.Builders.DockerUnavailableException)
        {
            // Docker not running: use fallback connection string (env or LocalDB)
            connectionString = Environment.GetEnvironmentVariable("TEST_CONNECTION_STRING")
                ?? "Server=(localdb)\\mssqllocaldb;Database=ContosoUniversity_Test;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true";

            await EnsureDatabaseMigratedAsync(connectionString);
        }

        _connectionString = connectionString;
        
        // Use environment variable to override connection string for Minimal APIs
        Environment.SetEnvironmentVariable("ConnectionStrings__db", connectionString);

        _factory = new ContosoTestApplicationFactory
        {
            ConnectionString = connectionString
        };

        _scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();

        _respawner = await Respawner.CreateAsync(connectionString, new RespawnerOptions
        {
            DbAdapter = DbAdapter.SqlServer,
            SchemasToInclude = new[] { "dbo" }
        });

        await _respawner.ResetAsync(connectionString);
    }

    /// <summary>
    /// Resets the database (deletes all data). Call from test class IAsyncLifetime to get a clean state per test class.
    /// </summary>
    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_connectionString);

        // Safeguard: Manually clear tables that might be missed or have identity/PK issues in some environments
        await ExecuteDbContextAsync(db => db.Database.ExecuteSqlRawAsync(@"
            DELETE FROM [Enrollment];
            DELETE FROM [CourseAssignment];
            DELETE FROM [Course];
            DELETE FROM [Student];
            DELETE FROM [Department];
            DELETE FROM [OfficeAssignment];
            DELETE FROM [Instructor];
        "));
    }

    private static async Task EnsureDatabaseMigratedAsync(string connectionString)
    {
        var options = new DbContextOptionsBuilder<SchoolContext>()
            .UseSqlServer(connectionString)
            .Options;

        await using (var context = new SchoolContext(options))
        {
            await context.Database.MigrateAsync();
        }
    }

    private async Task<string> InitializeWithDockerAsync()
    {
        // Create a Docker network for container-to-container communication
        _network = new NetworkBuilder()
            .WithName($"test-net-{Guid.NewGuid():N}")
            .Build();

        await _network.CreateAsync();

        // Start SQL Server on the network with a known alias
        _msSqlContainer = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-CU10-ubuntu-22.04")
            .WithNetwork(_network)
            .WithNetworkAliases("mssql")
            .Build();

        await _msSqlContainer.StartAsync();

        var connectionString = _msSqlContainer.GetConnectionString();

        // Run Grate migrations using the erikbra/grate Docker image
        // The Grate container connects to SQL via the network alias "mssql"
        var builder = new SqlConnectionStringBuilder(connectionString)
        {
            DataSource = "mssql,1433"
        };
        var inContainerConn = builder.ToString();

        // Find and mount the migration scripts directory
        var hostScriptsPath = Path.GetFullPath(Path.Combine(FindRepoRoot(Directory.GetCurrentDirectory()), "ContosoUniversity", "App_Data"));
        var mountPath = ToDockerPath(hostScriptsPath);

        var grateContainer = new ContainerBuilder()
            .WithImage("erikbra/grate:1.8.0")
            .WithNetwork(_network)
            .WithBindMount(mountPath, "/db")
            .WithEnvironment("APP_CONNSTRING", inContainerConn)
            .WithEnvironment("DATABASE_TYPE", "sqlserver")
            .WithWaitStrategy(Wait
                .ForUnixContainer()
                .UntilMessageIsLogged("has grated your database",
                    strategy => strategy.WithMode(WaitStrategyMode.OneShot)
                        .WithTimeout(TimeSpan.FromMinutes(2))))
            .Build();

        try
        {
            await grateContainer.StartAsync();

            var (stdout, stderr) = await grateContainer.GetLogsAsync();

            if (stderr.Contains("error", StringComparison.OrdinalIgnoreCase) ||
                stderr.Contains("failed", StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception("Grate migrations failed. Logs:\n" + stderr);
            }
        }
        finally
        {
            await grateContainer.StopAsync();
            await grateContainer.DisposeAsync();
        }

        return connectionString;
    }

    private static string ToDockerPath(string path)
    {
        var full = Path.GetFullPath(path);
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            full = full.Replace('\\', '/');
            if (full.Length > 1 && full[1] == ':')
                full = "/" + char.ToLower(full[0]) + full.Substring(2);
        }
        return full;
    }

    private static string FindRepoRoot(string start)
    {
        var dir = new DirectoryInfo(start);

        while (dir != null)
        {
            if (File.Exists(Path.Combine(dir.FullName, "ContosoUniversity.sln")))
                return dir.FullName;

            dir = dir.Parent;
        }

        // fallback to the starting directory if not found
        return start;
    }

    public async Task DisposeAsync()
    {
        if (_msSqlContainer != null)
        {
            await _msSqlContainer.StopAsync();
            await _msSqlContainer.DisposeAsync();
        }
        
        if (_factory != null)
        {
            await _factory.DisposeAsync();
        }
        
        if (_network != null)
        {
            await _network.DeleteAsync();
            await _network.DisposeAsync();
        }
    }
}
