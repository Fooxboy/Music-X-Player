using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public override void OnScrolled(RecyclerView recyclerView, int dx, int dy)
        {
            base.OnScrolled(recyclerView, dx, dy);

            var lm = (LinearLayoutManager)recyclerView.GetLayoutManager();
            if (lm.FindLastCompletelyVisibleItemPosition() == recyclerView.GetAdapter().ItemCount - 1)
            {
                //МЫ НА ДНЕ СЛАВК
                Toast.MakeText(Application.Context, "МЫ НА ДНЕ", ToastLength.Long).Show();
            }
            /*int visibleItemCount = lm.ChildCount;//смотрим сколько элементов на экране
            int totalItemCount = lm.ItemCount;//сколько всего элементов
            int firstVisibleItems = lm.FindFirstVisibleItemPosition();//какая позиция первого элемента

            if (!isLoading)
            {//проверяем, грузим мы что-то или нет, эта переменная должна быть вне класса  OnScrollListener 
                if ((visibleItemCount + firstVisibleItems) >= totalItemCount)
                {
                    isLoading = true;//ставим флаг что мы попросили еще элемены
                    if (loadingListener != null)
                    {
                        loadingListener.loadMoreItems(totalItemCount);//тут я использовал калбэк который просто говорит наружу что нужно еще элементов и с какой позиции начинать загрузку
                    }
                }
            }*/

        }

    }
}