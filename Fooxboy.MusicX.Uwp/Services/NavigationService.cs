using Fooxboy.MusicX.Uwp.Resources.ContentDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Fooxboy.MusicX.Uwp.Services
{
    /// <summary>
    /// Сервис навигации по приложению.
    /// </summary>
    public class NavigationService
    {
        /// <summary>
        /// Страницы
        /// </summary>
        public Stack<Type> Pages = new Stack<Type>();
        /// <summary>
        /// Текущий фрейм
        /// </summary>
        public Frame RootFrame { get; set; }

        /// <summary>
        /// Перейти на страницу
        /// </summary>
        /// <param name="page">Страница</param>
        /// <param name="data">Данные</param>
        public void Go(Type page, object data = null)
        {
            try
            {
                if (this.Pages.Count > 0)
                {
                    if (this.Pages.Peek() == page) return;
                }
                this.Pages.Push(page);
                this.UpdateButtonBack();
                this.RootFrame.CacheSize = 3;
                this.RootFrame.Navigate(page, data);
            }catch(Exception e)
            {
                //ContentDialogService.Show(new ExceptionDialog("Невозможно перейти на страницу", "Попробуйте перезапустить приложение", e), 1);
            }
            
        }

        /// <summary>
        /// Вернуться назад
        /// </summary>
        public void Back()
        {
            if (Pages.Count >= 2)
            {
                try
                {
                    this.RootFrame.GoBack();
                }
                catch
                {

                }
            }
            this.Pages.Pop();
            this.UpdateButtonBack();
        }

        /// <summary>
        /// Обновление кнопки назад
        /// </summary>
        void UpdateButtonBack()
        {
            SystemNavigationManager.GetForCurrentView().
                AppViewBackButtonVisibility = Pages.Count>= 2 ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
        }
    }
}
