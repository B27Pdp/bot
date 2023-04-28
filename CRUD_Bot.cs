using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace CRUD
{
    internal class CRUD_Bot
    {
        public static void Start()
        {
            TelegramBotClient botClient = new("6244191933:AAEhwlO6J86CsKuAGyl4107WzvsrRKElY0c");
            botClient.StartReceiving<UpdateHandler>();

            //botClient.ReceiveAsync<UpdateHandler>();
            Console.ReadKey();
        }
    }
    internal class UpdateHandler : IUpdateHandler
    {
        string InnerCommand = "";
        User user1 = new();
        Context context = new();
        public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {

            await botClient.SendTextMessageAsync(botClient.BotId, "error");
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            await Console.Out.WriteLineAsync(update.Message?.Text);
            long chatId = update.Message.Chat.Id;
            string? command = update.Message.Text;


            KeyboardButton StartButton = new KeyboardButton("Start");
            KeyboardButton AddButton = new KeyboardButton("Add");
            KeyboardButton UpdateButton = new KeyboardButton("Update");
            KeyboardButton DeleteButton = new KeyboardButton("Delete ");
            KeyboardButton GetButton = new KeyboardButton("GetAll");
            KeyboardButton GetAdminContact = new KeyboardButton("GetAdminContact");
            KeyboardButton SendContact = new KeyboardButton("SendContact")
            {
                RequestContact = true,
                RequestLocation = true
            };
            ReplyKeyboardMarkup markup = new(StartButton);

            var keyboard2 = new ReplyKeyboardMarkup(new[]
            {
                new []
                {
                    GetButton, AddButton
                },
                new []
                {
                    UpdateButton, DeleteButton
                },
                new []
                {
                    GetAdminContact, SendContact
                }
            });
            if (command.StartsWith("/start"))
            {
                await botClient.SendPhotoAsync(chatId, new InputOnlineFile("https://yandex.ru/images/search?pos=2&from=tabbar&img_url=http%3A%2F%2Fpngitem.com%2Fpimgs%2Fm%2F158-1585435_ai-bot-png-transparent-png.png&text=start+bot+image&rpt=simage&lr=194600"));
                await botClient.SendTextMessageAsync(chatId, "Bu bot malumotlar bazasi sifatida xizmat qiladi", replyMarkup: keyboard2);
            }
            

            switch (InnerCommand)
            {
                case "Delete":
                    {

                        InnerCommand = "";
                        var user = await context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == update.Message.Text);
                        if (user is not null)
                        {
                            context.Users.Remove(user);
                            await context.SaveChangesAsync();
                            await botClient.SendTextMessageAsync(chatId, "successfully deleted", replyMarkup: keyboard2);
                        }
                        else await botClient.SendTextMessageAsync(chatId, "there is no user with this number. ", replyMarkup: keyboard2);
                        break;
                    }
                case "AddContactName":
                    {
                        InnerCommand = "AddPhoneNumber";
                        user1.Name= update.Message.Text;
                        await botClient.SendTextMessageAsync(chatId, "SendPhoneNumber\nExample: +998901234567");
                        break;
                    }
                case "AddPhoneNumber":
                    {
                        InnerCommand = "";
                        user1.PhoneNumber= update.Message.Text;
                        await context.Users.AddAsync(user1 as User);
                        await context.SaveChangesAsync();
                        await botClient.SendTextMessageAsync(chatId, "Succsesfully added");
                        break;
                    }
                case "Update":
                    {
                        user1 = await context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == update.Message.Text);
                        if (user1 is not null)
                        {
                            InnerCommand = "AddContactName";
                            context.Users.Remove(user1 as User);
                            await botClient.SendTextMessageAsync(chatId, "Send ContactName");
                        }
                        break;
                    }

            }
            switch (command)
            {
                case "GetAll":
                    foreach (var user in context.Users.ToList())
                    {
                        await botClient.SendContactAsync(chatId, user.PhoneNumber, user.Name);
                    }
                    break;
                case "GetAdminContact":
                    {
                        await botClient.SendContactAsync(chatId, "+998903409342", "Javlon Usmonov");
                        break;
                    }
                case "Delete":
                    {
                        InnerCommand = "Delete";
                        Console.WriteLine("Deleted: "+update.Message.Text);
                        await botClient.SendTextMessageAsync(chatId, "Send PhoneNumber");
                        break;
                    }
                case "Add":
                    {
                        InnerCommand = "AddContactName";
                        await botClient.SendTextMessageAsync(chatId, "Send ContactName");
                        break;
                    }
                case "Update":
                    {
                        InnerCommand = "Update";
                        await botClient.SendTextMessageAsync(chatId, "Send NewPhoneNumber");
                        break;
                    }
            }
        }

        private object InlineKeyboardMarkup(InlineKeyboardButton[][] inlineKeyboardButtons)
        {
            throw new NotImplementedException();
        }
    }
}
