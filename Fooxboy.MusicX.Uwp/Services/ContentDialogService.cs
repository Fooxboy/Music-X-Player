﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Threading;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Fooxboy.MusicX.Uwp.Services
{
    public static class ContentDialogService
    {
        public async static Task Show(ContentDialog dialog)
        { 
            DispatcherHelper.CheckBeginInvokeOnUI(async () =>
            {
                var openPopups = VisualTreeHelper.GetOpenPopups(Window.Current);
                bool open = false;
                foreach (var popup in openPopups)
                {
                    if (popup.Child is ContentDialog)
                    {
                        open = true;
                    }
                }

                try
                {
                    if (!open) await dialog.ShowAsync();

                }catch(Exception e)
                {
                    //???????????? И как вывести ошибку
                }
            });
            
        }

        public static void Show(ContentDialog dialog, int a = 0)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(async () =>
            {
                var openPopups = VisualTreeHelper.GetOpenPopups(Window.Current);
                bool open = false;
                foreach (var popup in openPopups)
                {
                    if (popup.Child is ContentDialog)
                    {
                        open = true;
                    }
                }

                try
                {
                    if (!open) await dialog.ShowAsync();

                }
                catch (Exception e)
                {
                    //???????????? И как вывести ошибку
                }
            });

        }
    }
}