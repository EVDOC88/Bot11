using Bot11.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot11.Controllers
{
    public class TextMessageController
    {
        private readonly ITelegramBotClient _telegramClient;
        private readonly IStorage _memoryStorage;

        public TextMessageController(ITelegramBotClient telegramBotClient, IStorage memoryStorage)
        {
            _telegramClient = telegramBotClient;
            _memoryStorage = memoryStorage;
        
            
        }
        public async Task Handle(Message message, CancellationToken ct)
        {
            switch (message.Text)
            {
                case "/start":

                    // Объект, представляющий кноки
                    var buttons = new List<InlineKeyboardButton[]>();
                    buttons.Add(new[]
                    {
                        InlineKeyboardButton.WithCallbackData($" Счет символов" , $"kol"),
                        InlineKeyboardButton.WithCallbackData($" Сумма чисел" , $"sum")
                    });

                    // передаем кнопки вместе с сообщением (параметр ReplyMarkup)
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"<b>  Мой бот имеет две функции.</b> {Environment.NewLine}" +
                        $"{Environment.NewLine}Можно посчитать кол-во сиволов в сообщении или сумму чисел{Environment.NewLine}", cancellationToken: ct, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));

                    break;

      

            }
            string userSelect = _memoryStorage.GetSession(message.Chat.Id).Select; // Здесь получим выбор функции
            if (userSelect == "kol")
            {
                await _telegramClient.SendTextMessageAsync(message.From.Id, $"Длина сообщения: {message.Text.Length} знаков", cancellationToken: ct);
            }
            if (userSelect == "sum")
            
            {
                string[] sum = message.Text.Split(' ');
                double summa = 0;
                bool error = false;
                foreach (string s2 in sum)
                {
                    try
                    {
                        summa += Convert.ToDouble(s2);
                    }
                    catch (Exception ex)
                    {
                        error = true;
                        await _telegramClient.SendTextMessageAsync(message.From.Id, $"Ошибка, повторите ввод! Нужно ввести числа через пробел, вы ввели не число!", cancellationToken: ct);
                    }
                }
                if (!error) 
                { 
                    await _telegramClient.SendTextMessageAsync(message.From.Id, $"Сумма чисел равна: {summa} ", cancellationToken: ct);
                }
            }

        }
    }
}
