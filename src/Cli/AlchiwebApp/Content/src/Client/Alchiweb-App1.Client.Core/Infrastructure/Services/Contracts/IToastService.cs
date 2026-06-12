using System;
using System.Collections.Generic;
using System.Text;

namespace Alchiweb-App1.Client.Core.Infrastructure.Services.Contracts;

public interface IToastService
{
    public void Info(string message);
    public void Success(string message);
    public void Warning(string message);
    public void Error(string message);
}
