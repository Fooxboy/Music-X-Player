using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Services.Store;
using Windows.UI.Popups;

namespace Fooxboy.MusicX.Uwp.Services
{
    public static class StoreService
    {
        private static StoreContext context = null;
        public static async Task<bool> IsBuyPro()
        {
            if (context == null)
            {
                context = StoreContext.GetDefault();
            }

            var a = await context.GetAppLicenseAsync();
            var licenses = a.AddOnLicenses;


            foreach (KeyValuePair<string, StoreLicense> item in licenses)
            {
                if (item.Key == "9NQXP4JCLXWB") return true;
            }

            return false;
        }


        public async static Task PurchaseAddOn(string storeId)
        {
            if (context == null)
            {
                context = StoreContext.GetDefault();
            }

            StorePurchaseResult result = await context.RequestPurchaseAsync(storeId);
            string extendedError = string.Empty;
            if (result.ExtendedError != null)
            {
                extendedError = result.ExtendedError.Message;
            }

            switch (result.Status)
            {
                case StorePurchaseStatus.AlreadyPurchased:
                    await new MessageDialog("Music X Pro уже куплен!", "Покупка Music X Pro").ShowAsync();
                    break;

                case StorePurchaseStatus.Succeeded:
                    await new MessageDialog("Music X Pro успешно куплен! Для применения изменений, перезапустите приложение", "Покупка Music X Pro").ShowAsync();
                    StaticContent.IsPro = true;
                    break;

                case StorePurchaseStatus.NotPurchased:
                    await new MessageDialog($"Произошла ошибка при покупке Music X Pro: {extendedError}", "Покупка Music X Pro").ShowAsync();
                    break;

                case StorePurchaseStatus.NetworkError:
                    await new MessageDialog($"Невозможно купить Music X Pro из-за проблем с интернет-подключением", "Покупка Music X Pro").ShowAsync();
                    break;

                case StorePurchaseStatus.ServerError:
                    await new MessageDialog($"Произошла ошибка на серере при покупке Music X Pro: {extendedError}", "Покупка Music X Pro").ShowAsync();
                    break;

                default:
                    await new MessageDialog($"Произошла неизвестная на серере при покупке Music X Pro: {extendedError}", "Покупка Music X Pro").ShowAsync();
                    break;
            }
        }
    }


}
