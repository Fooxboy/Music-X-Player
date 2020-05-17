using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using DryIoc;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Fooxboy.MusicX.Core;

namespace Fooxboy.MusicX.Uwp.Services
{
    public class LoggerService:ILoggerService
    {

        public LoggerService()
        {
            Messages = new List<string>();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMinutes(2);
            timer.Tick += TimerTick;
            timer.Start();

        }
        private List<string> Messages { get; set; }
        public void Info(object msg)
        {
            Write($"[INFO]=> {msg}");
        }

        public void Trace(object msg)
        {
            Write($"[TRACE]=> {msg}");

        }

        public void Error(object msg, Exception e)
        {
            Write($"[ERROR]=> {msg} \n EXCEPTION: {e}");

        }

        public async Task SaveLog()
        {
            var savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            savePicker.FileTypeChoices.Add("Music X Log", new List<string>() { ".log" });
            savePicker.SuggestedFileName = $"Log {DateTime.Now.Day}.{DateTime.Now.Month}.{DateTime.Now.Year}";
            var c = Container.Get;
            var notification = c.Resolve<NotificationService>();
            var file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
               
                Windows.Storage.CachedFileManager.DeferUpdates(file);
                IsNotSaved = true;

                await Task.Run(() =>
                {
                     TimerTick(null, null);
                });

                var localpath = ApplicationData.Current.LocalFolder;


                while (IsNotSaved)
                {
                   Thread.Sleep(100);
                }

                var fileLog =
                    await localpath.GetFileAsync(
                        $"Log {DateTime.Now.Day}.{DateTime.Now.Month}.{DateTime.Now.Year}.log");
                var str = await FileIO.ReadTextAsync(fileLog);
                await FileIO.WriteTextAsync(file, str);
             
                var status =
                    await CachedFileManager.CompleteUpdatesAsync(file);
                if (status == Windows.Storage.Provider.FileUpdateStatus.Complete)
                {
                  notification.CreateNotification("Лог сохранен", "Файл лога успешно сохранен.");
                }
                else
                {
                  notification.CreateNotification("Лог не сохранен", $"Статус сохранения: {status}.");

                }
            }
            else
            {
                  notification.CreateNotification("Лог не сохранен", $"Лог не может быть сохранен.");
            }
        }

        private bool IsNotSaved = true;

        private void Write(string msg)
        {
            var s = $"({DateTime.Now}){msg}";
            Debug.WriteLine(s);
            Messages.Add(s);
        }

        private async void TimerTick(object sender, object e)
        {
            var localpath = ApplicationData.Current.LocalFolder;
            var filePath = $"Log {DateTime.Now.Day}.{DateTime.Now.Month}.{DateTime.Now.Year}.log";
            var item = await localpath.TryGetItemAsync(filePath);
            IStorageFile file;
            if (item == null)
            {
                file = await localpath.CreateFileAsync(filePath);
            }
            else
            { 
                file = await localpath.GetFileAsync(filePath);
                var str = await FileIO.ReadLinesAsync(file);
                List<string> array = new List<string>();
                array.AddRange(str);
                array.AddRange(Messages);
                await FileIO.WriteLinesAsync(file, array);
                Messages.Clear();
                IsNotSaved = false;
            }
        }
    }
}
