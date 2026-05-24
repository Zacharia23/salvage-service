namespace SalvageCore.Helpers;

public class EmailTemplates
{
    public string GetVerificationEmail(string email, string code)
    {
        return $@"
       
        <!DOCTYPE html>
            <html lang=""en"">
            <head>
                <meta charset=""UTF-8"">
                <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                <title>Help us protect your account</title>
                <style>
                    * {{
                        margin: 0;
                        padding: 0;
                        box-sizing: border-box;
                    }}
                    
                    body {{
                        font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, 'Helvetica Neue', Arial, sans-serif;
                        background-color: #1f1f1f;
                        color: #ffffff;
                        padding: 40px 20px;
                        line-height: 1.6;
                    }}
                    
                    .container {{
                        max-width: 600px;
                        margin: 0 auto;
                    }}
                    
                    .logo {{
                        text-align: center;
                        margin-bottom: 40px;
                    }}
                    
                    .logo svg {{
                        width: 80px;
                        height: 80px;
                    }}
                    
                    .card {{
                        background-color: #2b2b2b;
                        border: 1px solid #3f3f3f;
                        border-radius: 8px;
                        padding: 50px 40px;
                        text-align: center;
                    }}
                    
                    .card h1 {{
                        font-size: 24px;
                        font-weight: 400;
                        margin-bottom: 20px;
                        color: #ffffff;
                    }}
                    
                    .card p {{
                        color: #d4d4d4;
                        font-size: 15px;
                        line-height: 1.5;
                        margin-bottom: 30px;
                    }}
                    
                    .verification-code {{
                        font-size: 36px;
                        font-weight: 400;
                        letter-spacing: 4px;
                        color: #ffffff;
                        margin: 30px 0;
                        font-family: 'Courier New', Courier, monospace;
                    }}
                    
                    .info-text {{
                        font-size: 14px;
                        color: #b0b0b0;
                        line-height: 1.6;
                        margin-top: 30px;
                    }}
                    
                    .info-text a {{
                        color: #5b9dd9;
                        text-decoration: none;
                    }}
                    
                    .info-text a:hover {{
                        text-decoration: underline;
                    }}
                    
                    .footer {{
                        text-align: center;
                        margin-top: 40px;
                        padding-top: 30px;
                    }}
                    
                    .footer-logo {{
                        display: flex;
                        align-items: center;
                        justify-content: center;
                        gap: 8px;
                        margin-bottom: 15px;
                    }}
                    
                    .footer-logo svg {{
                        width: 24px;
                        height: 24px;
                    }}
                    
                    .footer-logo span {{
                        font-size: 20px;
                        font-weight: 400;
                        color: #b0b0b0;
                    }}
                    
                    .footer-links {{
                        font-size: 13px;
                        color: #8a8a8a;
                    }}
                    
                    .footer-links a {{
                        color: #5b9dd9;
                        text-decoration: none;
                        margin: 0 5px;
                    }}
                    
                    .footer-links a:hover {{
                        text-decoration: underline;
                    }}
                </style>
            </head>
            <body>
                <div class=""container"">
                    <!-- Logo -->
                    <div class=""logo"">
                       
                    </div>
                    
                    <!-- Card -->
                    <div class=""card"">
                        <h1>Help us protect your account</h1>
                        <p>Before you sign in, we need to verify your identity. Enter the<br>following code on the sign-in page.</p>
                        
                        <div class=""verification-code"">{code}</div>
                        
                        <div class=""info-text"">
                            If you have not recently tried to sign into GitLab, we recommend 
                            <a href=""#"">changing your password</a> and 
                            <a href=""#"">setting up Two-Factor Authentication</a> to keep your account safe.<br>
                            Your verification code expires after 10 minutes.
                        </div>
                    </div>
                    
                    <!-- Footer -->
                    <div class=""footer"">
                        <div class=""footer-logo"">
                         
                            <span>Smart Salvage Tanzania</span>
                        </div>
                        <div class=""footer-links"">
                            You're receiving this email because of your account on 
                            <a href=""#"">https://smartsalvagetz.com/</a>. 
                            <a href=""#"">Manage all notifications</a> · 
                            <a href=""#"">Help</a>
                        </div>
                    </div>
                </div>
            </body>
            </html>
        ";
    }

    public string GetWelcomeMessage(string? email, string name)
    {
        return $@"
            <!DOCTYPE html>
            <html lang=""en"">
            <head>
                <meta charset=""UTF-8"">
                <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                <title>Welcome to Smart Salvage Tanzania</title>
                <style>
                    * {{
                        margin: 0;
                        padding: 0;
                        box-sizing: border-box;
                    }}
                    
                    body {{
                        font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, 'Helvetica Neue', Arial, sans-serif;
                        background-color: #1f1f1f;
                        color: #ffffff;
                        padding: 40px 20px;
                        line-height: 1.6;
                    }}
                    
                    .container {{
                        max-width: 600px;
                        margin: 0 auto;
                    }}
                    
                    .logo {{
                        text-align: center;
                        margin-bottom: 40px;
                    }}
                    
                    .logo svg {{
                        width: 80px;
                        height: 80px;
                    }}
                    
                    .card {{
                        background-color: #2b2b2b;
                        border: 1px solid #3f3f3f;
                        border-radius: 8px;
                        padding: 50px 40px;
                        text-align: center;
                    }}
                    
                    .card h1 {{
                        font-size: 28px;
                        font-weight: 400;
                        margin-bottom: 20px;
                        color: #ffffff;
                    }}
                    
                    .card p {{
                        color: #d4d4d4;
                        font-size: 15px;
                        line-height: 1.6;
                        margin-bottom: 20px;
                    }}
                    
                    .welcome-icon {{
                        font-size: 64px;
                        margin-bottom: 20px;
                    }}
                    
                    .cta-button {{
                        display: inline-block;
                        background-color: #5b9dd9;
                        color: #ffffff;
                        padding: 12px 32px;
                        border-radius: 4px;
                        text-decoration: none;
                        font-size: 15px;
                        font-weight: 500;
                        margin: 20px 0;
                        transition: background-color 0.2s;
                    }}
                    
                    .cta-button:hover {{
                        background-color: #4a8bc2;
                    }}
                    
                    .features {{
                        text-align: left;
                        margin: 30px 0;
                        padding: 0 20px;
                    }}
                    
                    .feature-item {{
                        display: flex;
                        align-items: flex-start;
                        margin-bottom: 20px;
                        gap: 15px;
                    }}
                    
                    .feature-icon {{
                        font-size: 24px;
                        flex-shrink: 0;
                    }}
                    
                    .feature-content h3 {{
                        font-size: 16px;
                        font-weight: 500;
                        color: #ffffff;
                        margin-bottom: 5px;
                    }}
                    
                    .feature-content p {{
                        font-size: 14px;
                        color: #b0b0b0;
                        margin: 0;
                    }}
                    
                    .divider {{
                        height: 1px;
                        background-color: #3f3f3f;
                        margin: 30px 0;
                    }}
                    
                    .info-text {{
                        font-size: 14px;
                        color: #b0b0b0;
                        line-height: 1.6;
                        margin-top: 30px;
                    }}
                    
                    .info-text a {{
                        color: #5b9dd9;
                        text-decoration: none;
                    }}
                    
                    .info-text a:hover {{
                        text-decoration: underline;
                    }}
                    
                    .footer {{
                        text-align: center;
                        margin-top: 40px;
                        padding-top: 30px;
                    }}
                    
                    .footer-logo {{
                        display: flex;
                        align-items: center;
                        justify-content: center;
                        gap: 8px;
                        margin-bottom: 15px;
                    }}
                    
                    .footer-logo svg {{
                        width: 24px;
                        height: 24px;
                    }}
                    
                    .footer-logo span {{
                        font-size: 20px;
                        font-weight: 400;
                        color: #b0b0b0;
                    }}
                    
                    .footer-links {{
                        font-size: 13px;
                        color: #8a8a8a;
                    }}
                    
                    .footer-links a {{
                        color: #5b9dd9;
                        text-decoration: none;
                        margin: 0 5px;
                    }}
                    
                    .footer-links a:hover {{
                        text-decoration: underline;
                    }}
                </style>
            </head>
            <body>
                <div class=""container"">
                    <!-- Logo -->
                    <div class=""logo"">
                       
                    </div>
                    
                    <!-- Card -->
                    <div class=""card"">
                        <div class=""welcome-icon"">👋</div>
                        
                        <h1>Welcome to Smart Salvage Tanzania!</h1>
                        
                        <p>Hi {name}! We're excited to have you on board. Your account has been successfully created and you're ready to start your journey with us.</p>
                        
                        <a href=""#"" class=""cta-button"">Get Started</a>
                        
                        <div class=""divider""></div>
                        
                        <div class=""features"">
                            <div class=""feature-item"">
                                <div class=""feature-icon"">🚀</div>
                                <div class=""feature-content"">
                                    <h3>Start Your First Project</h3>
                                    <p>Create repositories, collaborate with your team, and bring your ideas to life.</p>
                                </div>
                            </div>
                            
                            <div class=""feature-item"">
                                <div class=""feature-icon"">🔒</div>
                                <div class=""feature-content"">
                                    <h3>Secure Your Account</h3>
                                    <p>Enable two-factor authentication to add an extra layer of security to your account.</p>
                                </div>
                            </div>
                            
                            <div class=""feature-item"">
                                <div class=""feature-icon"">👥</div>
                                <div class=""feature-content"">
                                    <h3>Invite Your Team</h3>
                                    <p>Collaboration is better together. Invite team members to join your projects.</p>
                                </div>
                            </div>
                            
                            <div class=""feature-item"">
                                <div class=""feature-icon"">📚</div>
                                <div class=""feature-content"">
                                    <h3>Explore Documentation</h3>
                                    <p>Check out our comprehensive guides and tutorials to get the most out of GitLab.</p>
                                </div>
                            </div>
                        </div>
                        
                        <div class=""divider""></div>
                        
                        <div class=""info-text"">
                            Need help getting started? Visit our <a href=""#"">Help Center</a> or check out our 
                            <a href=""#"">Getting Started Guide</a>. Our <a href=""#"">Support Team</a> is always here to help.
                        </div>
                    </div>
                    
                    <!-- Footer -->
                    <div class=""footer"">
                        <div class=""footer-logo"">
                          
                            <span>Smart Salvage Tanzania</span>
                        </div>
                        <div class=""footer-links"">
                            You're receiving this email because you created an account on 
                            <a href=""#"">smartsalvagetz.com</a>. 
                            <a href=""#"">Manage all notifications</a> · 
                            <a href=""#"">Help</a>
                        </div>
                    </div>
                </div>
            </body>
            </html>
        ";
    }
}