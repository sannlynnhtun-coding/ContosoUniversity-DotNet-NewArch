var builder = DistributedApplication.CreateBuilder(args);

var sql = builder.AddSqlServer("sql")
    .WithLifetime(ContainerLifetime.Persistent);

var db = sql.AddDatabase("contosouniversity-db")
    .WithCommand(
        name: "migrate-contosouniversity-db",
        displayName: "Migrate ContosoUniversity Database",
        executeCommand: async cmd =>
        {
            var process = new System.Diagnostics.Process();
            process.StartInfo.WorkingDirectory = "../../ContosoUniversity/ContosoUniversity.Data";
            process.StartInfo.FileName = "dotnet";
            process.StartInfo.Arguments = $"grate -c -f ContosoUniversity/App_Data";
            process.StartInfo.UseShellExecute = true;

            process.Start();

            await process.WaitForExitAsync(cmd.CancellationToken);

            return CommandResults.Success();
        },
        commandOptions: new CommandOptions { IconName = "ArrowSquareUpRight" }
    );
    ;

builder.AddProject<Projects.ContosoUniversity>("contosouniversity")
    .WithReference(db)
    .WaitFor(db);

builder.Build().Run();
