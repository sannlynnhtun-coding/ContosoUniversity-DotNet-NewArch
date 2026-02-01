using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ContosoUniversity.IntegrationTests;

/// <summary>
/// Base class for slice tests. Resets the database and sets a unique course ID base before each test class runs
/// so tests get a clean state and Course IDs do not collide when test classes run in parallel.
/// </summary>
public abstract class SliceTestBase : IAsyncLifetime
{
    private static int _classCounter;

    protected SliceFixture Fixture { get; }

    protected SliceTestBase(SliceFixture fixture) => Fixture = fixture;

    public virtual async Task InitializeAsync()
    {
        Fixture.SetCourseNumberBase(Interlocked.Increment(ref _classCounter) * 10_000);
        await Fixture.ResetDatabaseAsync();
    }

    public virtual Task DisposeAsync() => Task.CompletedTask;
}
