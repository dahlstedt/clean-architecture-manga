namespace Manga.Application.Boundaries.Withdraw
{
    public interface IOutputHandler : IErrorHandler
    {
        void Handle(Output output);
    }
}