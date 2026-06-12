using System;
using System.Collections.Generic;
using System.Text;
using AntDesign;

namespace Alchiweb-App1.Client.Core.Infrastructure.Services;

public class ToastService(IMessageService _messageService) : IToastService
{
    public void Info(string message) => _messageService.Info(message);

    public void Success(string message) => _messageService.Success(message);

    public void Warning(string message) => _messageService.Warning(message);

    public void Error(string message) => _messageService.Error(message);
}
