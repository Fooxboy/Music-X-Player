using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using static Android.Support.V7.Widget.RecyclerView;

namespace Fooxboy.MusicX.AndroidApp.Listeners
{
    public class OnScrollToBottomListener: RecyclerView.OnScrollListener
    {
        private Action callback;
        private bool isLoading = false;
        public OnScrollToBottomListener(Action a)
        {
            callback = a;
        }

        public void InvokeCallback()
        {
            //Toast.MakeText(Application.Context, "МЫ НА ДНЕ", ToastLength.Long).Show();

            if (isLoading) return;
            isLoading = true;
            Task.Run(() =>
            {
                callback?.Invoke();
                isLoading = false;
            });
        }

        public override void OnScrolled(RecyclerView recyclerView, int dx, int dy)
        {
            base.OnScrolled(recyclerView, dx, dy);

            var lm = (LinearLayoutManager)recyclerView.GetLayoutManager();
            if (lm.FindLastCompletelyVisibleItemPosition() == recyclerView.GetAdapter().ItemCount - 1)
            {
                //Toast.MakeText(Application.Context, "МЫ НА ДНЕ", ToastLength.Long).Show();

                if (isLoading) return;
                isLoading = true;
                Task.Run(() =>
                {
                    callback?.Invoke();
                    isLoading = false;
                });
                

                //МЫ НА ДНЕ СЛАВК
            }
        }

    }
}