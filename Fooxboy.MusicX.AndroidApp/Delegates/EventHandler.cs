namespace Fooxboy.MusicX.AndroidApp.Delegates
{
    public delegate void EventHandler<T>(object sender, T args);
    public delegate void EventHandler<T, T2>(object sender, T args, T2 block);
}