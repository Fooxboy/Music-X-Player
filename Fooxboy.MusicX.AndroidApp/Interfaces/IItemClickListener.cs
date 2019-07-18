using Android.Views;

namespace Fooxboy.MusicX.AndroidApp.Interfaces
{
    public interface IItemClickListener
    {
        void OnClick(View itemView, int position, bool isLongClick);
    }
}