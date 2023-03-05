using Bot11.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Bot11.Controllers
{
  public class InlineKeyboardController
    {
        private readonly IStorage _memoryStorage;
        private readonly ITelegramBotClient _telegramClient;

        public InlineKeyboardController(ITelegramBotClient telegramBotClient, IStorage memoryStorage)
        {
            _telegramClient = telegramBotClient;
            _memoryStorage = memoryStorage;
        }

        public async Task Handle(CallbackQuery? callbackQuery, CancellationToken ct)
        {
            if (callbackQuery?.Data == null)
                return;

            // Обновление пользовательской сессии новыми данными
            _memoryStorage.GetSession(callbackQuery.From.Id).Select = callbackQuery.Data;

            // Генерим информационное сообщение
            string Select = callbackQuery.Data switch
            {
                "sum" => " Сумма чисел",
                "kol" => " Счет символов",
                _ => String.Empty
            };

            // Отправляем в ответ уведомление о выборе
            await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id,
                $"<b>Функцию - {Select}.{Environment.NewLine}</b>" +
                $"{Environment.NewLine} Функцию можно поменять в главном меню.", cancellationToken: ct, parseMode: ParseMode.Html);
            if (callbackQuery.Data == "kol")
            await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id,
                 $"<b>{Environment.NewLine} Введите сообщение для которого нужно посчитать кол-во символов</b>", cancellationToken: ct, parseMode: ParseMode.Html);
            if (callbackQuery.Data == "sum")
                await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id,
                     $"<b>{Environment.NewLine} Введите числа через пробел</b>", cancellationToken: ct, parseMode: ParseMode.Html);

        }
    }
}
