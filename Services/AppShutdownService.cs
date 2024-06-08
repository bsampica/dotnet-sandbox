namespace dotnet_sandbox.Services;

public class AppShutdownService()
{
    private readonly CancellationTokenSource TokenSource = new CancellationTokenSource();
    public CancellationToken Token
    {
        get
        {
            return TokenSource.Token;
        }
        private set
        {
            // setup a new token
            Register();
        }
    }

    public void Register()
    {
        throw new NotImplementedException();
    }

    public void TriggerShutdown()
    {
        TokenSource.Cancel();
    }
}