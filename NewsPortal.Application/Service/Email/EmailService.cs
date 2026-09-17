using Microsoft.Extensions.Configuration;
using NewsPortal.Application.Interfaces;
using System.Net;
using System.Net.Mail;

namespace NewsPortal.Infrastructure.Services;

public sealed class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendPasswordResetEmailAsync(
        string email,
        string token)
    {
        var frontendBaseUrl = GetRequiredSetting("Frontend:BaseUrl");

        var encodedToken = Uri.EscapeDataString(token);

        var resetUrl =
            $"{frontendBaseUrl.TrimEnd('/')}/reset-password?token={encodedToken}";

        var subject = "بازیابی رمز عبور - NewsPortal";

        var htmlBody = $"""
            <!DOCTYPE html>
            <html lang="fa" dir="rtl">
            <head>
                <meta charset="UTF-8">
                <meta name="viewport" content="width=device-width, initial-scale=1.0">
                <title>{subject}</title>
            </head>
            <body style="font-family: Tahoma, Arial, sans-serif; direction: rtl; line-height: 1.8;">
                <h2>بازیابی رمز عبور</h2>

                <p>
                    درخواست بازیابی رمز عبور برای حساب کاربری شما در NewsPortal ثبت شده است.
                </p>

                <p>
                    برای تغییر رمز عبور روی لینک زیر کلیک کنید:
                </p>

                <p>
                    <a href="{HtmlEncode(resetUrl)}">
                        بازیابی رمز عبور
                    </a>
                </p>

                <p>
                    این لینک به مدت یک ساعت معتبر است.
                </p>

                <p>
                    اگر شما این درخواست را ارسال نکرده‌اید، این ایمیل را نادیده بگیرید.
                </p>
            </body>
            </html>
            """;

        await SendEmailAsync(email, subject, htmlBody);
    }

    public async Task SendEmailVerificationEmailAsync(
        string email,
        string token)
    {
        var frontendBaseUrl = GetRequiredSetting("Frontend:BaseUrl");

        var encodedToken = Uri.EscapeDataString(token);

        var verificationUrl =
            $"{frontendBaseUrl.TrimEnd('/')}/verify-email?token={encodedToken}";

        var subject = "تأیید ایمیل - NewsPortal";

        var htmlBody = $"""
            <!DOCTYPE html>
            <html lang="fa" dir="rtl">
            <head>
                <meta charset="UTF-8">
                <meta name="viewport" content="width=device-width, initial-scale=1.0">
                <title>{subject}</title>
            </head>
            <body style="font-family: Tahoma, Arial, sans-serif; direction: rtl; line-height: 1.8;">
                <h2>تأیید ایمیل</h2>

                <p>
                    برای تکمیل ثبت‌نام در NewsPortal، ایمیل خود را تأیید کنید.
                </p>

                <p>
                    <a href="{HtmlEncode(verificationUrl)}">
                        تأیید ایمیل
                    </a>
                </p>

                <p>
                    این لینک به مدت ۲۴ ساعت معتبر است.
                </p>

                <p>
                    اگر شما این حساب را ایجاد نکرده‌اید، این ایمیل را نادیده بگیرید.
                </p>
            </body>
            </html>
            """;

        await SendEmailAsync(email, subject, htmlBody);
    }

    private async Task SendEmailAsync(
        string recipient,
        string subject,
        string htmlBody)
    {
        var smtpHost = GetRequiredSetting("Email:SmtpHost");
        var smtpPortText = GetRequiredSetting("Email:SmtpPort");
        var username = GetRequiredSetting("Email:Username");
        var password = GetRequiredSetting("Email:Password");
        var fromEmail = GetRequiredSetting("Email:FromEmail");
        var fromName = GetRequiredSetting("Email:FromName");

        if (!int.TryParse(smtpPortText, out var smtpPort))
        {
            throw new InvalidOperationException(
                "Email:SmtpPort configuration is invalid.");
        }

        using var message = new MailMessage
        {
            From = new MailAddress(fromEmail, fromName),
            Subject = subject,
            Body = htmlBody,
            IsBodyHtml = true
        };

        message.To.Add(recipient);

        using var smtpClient = new SmtpClient(smtpHost, smtpPort)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(username, password)
        };

        await smtpClient.SendMailAsync(message);
    }

    private string GetRequiredSetting(string key)
    {
        var value = _configuration[key];

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException(
                $"Missing email configuration: {key}");
        }

        return value;
    }

    private static string HtmlEncode(string value)
    {
        return WebUtility.HtmlEncode(value);
    }
}