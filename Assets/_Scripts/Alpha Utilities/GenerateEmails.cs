using System.Collections.Generic;
using UnityEngine;

public static class GenerateEmails
{
    public class EmailScenario
    {
        public string Content { get; set; }
        public int AllowAccessPoints { get; set; }
        public int AskForIDPoints { get; set; }
        public int DenyAccessPoints { get; set; }
    }

    private static readonly List<string> genuineEmails = new()
    {
        "Hi dear,\n\n    Your password reset request has been received. Click the link below to reset your password:\n    https://secure.example.com/reset-password\n\nBest regards,\nYour Bank",
        "Hi dear,\n\n    We have noticed unusual activity on your account. Please log in to verify recent transactions:\n    https://www.bank.com/login\n\nBest regards,\nAccount Security Team",
        "Hi dear,\n\n    Thank you for contacting customer support. Your request has been processed successfully.\n\nBest regards,\nCustomer Support",
        "Hi dear,\n\n    Your subscription has been renewed successfully. Your next billing date is [Date].\n\nBest regards,\nSubscription Team",
        "Hi dear,\n\n    Your recent purchase of [Product] has been successfully completed. Thank you for shopping with us!\n\nBest regards,\nOnline Store",
        "Hi dear,\n\n    Your account verification was successful. You can now access all features.\n\nBest regards,\nVerification Team",
        "Hi dear,\n\n    Your request for account deletion has been received. It will be processed in 7 days unless canceled.\n\nBest regards,\nSupport Team",
        "Hi dear,\n\n    Your package has been shipped and is on its way. Track it here:\n    https://www.shippingcompany.com/track\n\nBest regards,\nShipping Team",
        "Hi dear,\n\n    Your refund request has been approved. The amount will be credited to your account within 5-7 business days.\n\nBest regards,\nBilling Team",
        "Hi dear,\n\n    Your email preferences have been updated successfully. Thank you!\n\nBest regards,\nPreference Management Team",
        "Hi dear,\n\n    Your loyalty points summary for this month is attached. Redeem them now for exciting rewards.\n\nBest regards,\nRewards Team",
        "Hi dear,\n\n    Thank you for registering for [Event Name]. We look forward to seeing you there.\n\nBest regards,\nEvent Management Team",
        "Hi dear,\n\n    Your feedback has been received. We value your input and will work to improve our services.\n\nBest regards,\nCustomer Experience Team",
        "Hi dear,\n\n    Your account statement for [Month] is now available. Access it here:\n    https://secure.bank.com/statement\n\nBest regards,\nYour Bank",
        "Hi dear,\n\n    Your warranty claim has been approved. Please contact us for the next steps.\n\nBest regards,\nWarranty Support Team",
        "Hi dear,\n\n    Your payment for [Service/Product] has been processed successfully.\n\nBest regards,\nBilling Department",
        "Hi dear,\n\n    Your reservation for [Booking Details] has been confirmed. See you soon!\n\nBest regards,\nReservation Team",
        "Hi dear,\n\n    Your account settings have been updated as requested.\n\nBest regards,\nAccount Management Team",
        "Hi dear,\n\n    Thank you for updating your contact information. Our records are now up to date.\n\nBest regards,\nSupport Team",
        "Hi dear,\n\n    Your request for [specific service] has been received and is being processed. We will update you shortly.\n\nBest regards,\nService Team",
        "Hi dear,\n\n    Your digital download of [Product] is ready. Click here to download:\n    https://downloads.store.com/item\n\nBest regards,\nStore Team",
        "Hi dear,\n\n    We have received your application for [Position]. Thank you for your interest in our company.\n\nBest regards,\nHR Team",
        "Hi dear,\n\n    Your membership for [Club/Service] has been renewed. Enjoy our services!\n\nBest regards,\nMembership Team",
        "Hi dear,\n\n    Your password was changed successfully. If this wasn’t you, please contact us immediately.\n\nBest regards,\nSecurity Team",
        "Hi dear,\n\n    Your new device has been registered to your account. If this wasn’t you, click here:\n    https://www.securityalerts.com/report\n\nBest regards,\nSecurity Alerts",
        // Add more genuine emails as required to fill the list
    };

    private static readonly List<string> phishingEmails = new()
    {
        "Hi dear,\n\n    Your account balance has dropped below $0. Resolve it now:\n    http://fraudulent.example.com/fixbalance\n\nBest regards,\nAccount Management",
        "Hi dear,\n\n    You’ve been selected for a premium account upgrade. Activate it here:\n    http://fake-upgrade.example.com\n\nBest regards,\nPremium Team",
        "Hi dear,\n\n    You’ve been added to a shared document. Access it here:\n    http://phishing.example.com/sharedoc\n\nBest regards,\nCollaboration Team",
        "Hi dear,\n\n    Update your email password now to continue using your account:\n    http://fakeemail.example.com/update\n\nBest regards,\nEmail Security Team",
        "Hi dear,\n\n    Your device has been infected. Clean it immediately:\n    http://malicious.example.com/deviceclean\n\nBest regards,\nTech Support",
        "Hi dear,\n\n    Your bank account has been suspended due to unauthorized activity. Resolve it here:\n    http://phish-bank.example.com/resolve\n\nBest regards,\nBank Security Team",
        "Hi dear,\n\n    You’ve received a payment of $500. Claim it here:\n    http://fraudulentpay.example.com/claim\n\nBest regards,\nPayment Department",
        "Hi dear,\n\n    Your email account is almost full. Click here to increase storage:\n    http://malicious.example.com/storageupgrade\n\nBest regards,\nEmail Team",
        "Hi dear,\n\n    Your monthly invoice is ready. Download it here:\n    http://scammer.example.com/invoice\n\nBest regards,\nBilling Team",
        "Hi dear,\n\n    Your account security question needs updating. Update it here:\n    http://phishing.example.com/securityquestion\n\nBest regards,\nAccount Security",
        "Hi dear,\n\n    Your reward points are expiring soon. Redeem them here:\n    http://fakepoints.example.com/redeem\n\nBest regards,\nRewards Team",
        "Hi dear,\n\n    We couldn’t deliver your package. Reschedule delivery here:\n    http://malicious-site.example.com/delivery\n\nBest regards,\nDelivery Service",
        "Hi dear,\n\n    Your tax filing has errors. Fix them here:\n    http://fraud-tax.example.com/fixerrors\n\nBest regards,\nTax Department",
        "Hi dear,\n\n    Update your payment details to avoid service interruption:\n    http://scammer-payment.example.com/update\n\nBest regards,\nPayment Team",
        "Hi dear,\n\n    Your phone bill is overdue. Pay it now to avoid disconnection:\n    http://phish-phonebill.example.com/pay\n\nBest regards,\nBilling Support",
        "Hi dear,\n\n    Secure your account to avoid permanent suspension:\n    http://fake-secure.example.com/account\n\nBest regards,\nAccount Recovery Team",
        "Hi dear,\n\n    Confirm your identity to unlock premium features:\n    http://fraudulent-premium.example.com/confirm\n\nBest regards,\nPremium Team",
        "Hi dear,\n\n    A payment of $120 has been charged to your account. Dispute it here:\n    http://phishing-dispute.example.com\n\nBest regards,\nFraud Resolution Team",
        "Hi dear,\n\n    Your social media account will be deactivated unless verified:\n    http://fake-verification.example.com/social\n\nBest regards,\nSocial Media Team",
        "Hi dear,\n\n    You have an urgent message. Read it here:\n    http://maliciousmessage.example.com/read\n\nBest regards,\nMessage Center",
        "Hi dear,\n\n    Reset your password to avoid account deactivation:\n    http://scammer-password.example.com/reset\n\nBest regards,\nPassword Recovery Team",
        "Hi dear,\n\n    Your login attempt failed. Verify it here:\n    http://phishing-login.example.com/verify\n\nBest regards,\nAccount Access Team",
        "Hi dear,\n\n    Your card ending in 1234 has been charged $200. Review it here:\n    http://malicious-card.example.com/review\n\nBest regards,\nTransaction Support",
        "Hi dear,\n\n    Complete your account setup now for extra rewards:\n    http://fake-setup.example.com/complete\n\nBest regards,\nSetup Assistance Team",
        "Hi dear,\n\n    Your cloud storage is at risk. Protect your files here:\n    http://phishing-cloud.example.com/protect\n\nBest regards,\nCloud Security Team",
        "Hi dear,\n\n    We detected an issue with your recent payment. Fix it here:\n    http://fraudulentpayment.example.com/fix\n\nBest regards,\nBilling Assistance",
        "Hi dear,\n\n    Your account will be permanently deleted in 24 hours unless recovered here:\n    http://phish-delete.example.com/recover\n\nBest regards,\nAccount Recovery",
        "Hi dear,\n\n    Your recent purchase failed. Retry payment here:\n    http://malicious-payment.example.com/retry\n\nBest regards,\nPayment Gateway",
        "Hi dear,\n\n    Your loan application has been pre-approved. Confirm here:\n    http://fraudulent-loan.example.com/confirm\n\nBest regards,\nLoan Approval Team",
        "Hi dear,\n\n    Download the latest update for your software here:\n    http://maliciousupdate.example.com/download\n\nBest regards,\nSoftware Support",
        "Hi dear,\n\n    Your account will expire soon. Renew it here:\n    http://phishing-renewal.example.com\n\nBest regards,\nRenewal Department",
        "Hi dear,\n\n    Verify your address to continue receiving deliveries:\n    http://fake-delivery.example.com/verify\n\nBest regards,\nDelivery Support",
        "Hi dear,\n\n    Activate two-factor authentication to secure your account:\n    http://phish-2fa.example.com/activate\n\nBest regards,\nSecurity Support",
        "Hi dear,\n\n    Your account privileges have been revoked. Restore them here:\n    http://fraudulent-account.example.com/restore\n\nBest regards,\nPrivileges Team",
        "Hi dear,\n\n    Your online banking session has expired. Log in again here:\n    http://phish-banking.example.com/login\n\nBest regards,\nBanking Team",
        "Hi dear,\n\n    You’ve been chosen for a secret shopper program. Apply here:\n    http://fake-shopper.example.com/apply\n\nBest regards,\nSecret Shopper Team",
        "Hi dear,\n\n    You’ve been tagged in a photo. View it here:\n    http://malicious-photo.example.com/view\n\nBest regards,\nPhoto Tagging Service",
        "Hi dear,\n\n    Your antivirus subscription has expired. Renew it here:\n    http://phish-antivirus.example.com/renew\n\nBest regards,\nAntivirus Team",
        "Hi dear,\n\n    Your login was detected from a new location. Verify it here:\n    http://fraudulent-location.example.com/verify\n\nBest regards,\nLogin Alert Team",
        "Hi dear,\n\n    Your gift card is waiting! Claim it here:\n    http://fake-giftcard.example.com/claim\n\nBest regards,\nGift Card Center",
        "Hi dear,\n\n    A new device has been linked to your account. Report it here:\n    http://phishing-device.example.com/report\n\nBest regards,\nDevice Management",
        "Hi dear,\n\n    Your access to premium content has been suspended. Reactivate it here:\n    http://malicious-content.example.com/reactivate\n\nBest regards,\nContent Team",
        "Hi dear,\n\n    Your payment confirmation is pending. Confirm it here:\n    http://phish-confirm.example.com/payment\n\nBest regards,\nPayment Confirmation",
        "Hi dear,\n\n    A job offer is waiting for you. Accept it here:\n    http://fraud-job.example.com/accept\n\nBest regards,\nJob Placement Team"
    };

    /// <summary>
    /// Get a random genuine email.
    /// </summary>
    public static string GetRandomGenuineEmail => genuineEmails[Random.Range(0, genuineEmails.Count)];

    /// <summary>
    /// Get a random phishing email.
    /// </summary>
    public static string GetRandomPhishingEmail => phishingEmails[Random.Range(0, phishingEmails.Count)];


    // List to hold all the predefined email contents
    private static readonly List<string> se_emailContentsOld = new()
    {
        "Hi, I'm a colleague from another branch. I need access to the financial reports.",
        "Hello, I’m with IT. I need access to the secure server.\nI’ve forgotten my credentials, but this is urgent!",
        "Hey, the CEO needs these files urgently. Can you send me the access details?",
        "I work in HR and need your help accessing the employee database for an audit.",
        "I'm from tech support. There's an issue with your account, and I need to reset your password.",
        "I'm an external contractor working with your department. I need access to the shared drive.",
        "Urgent: Your account has been compromised. Please provide login details to secure it immediately.",
        "This is the billing department. We need your payment credentials to resolve an invoice issue.",
        "I'm the new intern. Can you grant me access to the marketing folder?",
        "I'm working late and lost my keycard. Could you give me remote access to the building systems?",
        "The finance department asked me to check some records. Can you send me the login details?",
        "Hi, we met at the conference. Can you give me access to the presentation material?",
        "I'm from the client team and need access to the project folder urgently.",
        "Hello, I’m from the security team. There’s an unusual activity on your account. Share your details to fix it.",
        "I’m the new manager in your team. Can you send me the credentials for the reporting dashboard?",
        "Hey, I’ve locked myself out of the database. Can you reset my access?",
        "Hi, I need access to the email server for a scheduled maintenance check.",
        "I’m from IT and need to confirm your login details for an update.",
        "The CTO needs access to the admin panel. Can you share the credentials?",
        "I’m working remotely and need to connect to the office network. Can you help?",
        "Hi, I’m your manager's assistant. I need access to the internal performance review documents.",
        "Hello, the accounting department needs your login details to verify an error in your last pay stub.",
        "Hey, I’m from the software team, and we need admin rights to install a new tool for your department.",
        "I’m working on a project and require access to the database of your past transactions.",
        "This is the building maintenance team. We need access to the server room to update the fire safety system.",
        "Hello, I’m the new compliance officer. Can you send me your team's workflow documents?",
        "I’ve noticed unusual activity in your account. Please confirm your recent login details for verification.",
        "The marketing department requested urgent access to your files for an upcoming campaign. Can you share them?",
        "Hi, I’m from customer service. There’s an unresolved complaint tied to your account. Can you provide access for review?",
        "The legal department is conducting an audit. Please share your access credentials to verify compliance records."

    };

    // Method to get a random email content
    public static string Get_SE_RandomEmailContent
        => se_emailContentsOld[Random.Range(0, se_emailContentsOld.Count)];

    private static readonly List<EmailScenario> se_scenarios = new()
    {
        new EmailScenario
        {
            Content = "Hi, I'm a colleague from another branch. I need access to the financial reports.",
            AllowAccessPoints = -10,
            AskForIDPoints = 10,
            DenyAccessPoints = 5
        },
        new EmailScenario
        {
            Content = "Hello, I’m with IT. I need access to the secure server.\nI’ve forgotten my credentials, but this is urgent!",
            AllowAccessPoints = -10,
            AskForIDPoints = 5,
            DenyAccessPoints = 10
        },
        new EmailScenario
        {
            Content = "Hey, the CEO needs these files urgently. Can you send me the access details?",
            AllowAccessPoints = -15,
            AskForIDPoints = 5,
            DenyAccessPoints = 10
        },

        new EmailScenario
        {
            Content = "Hi, I’m your manager's assistant. I need access to the internal performance review documents.",
            AllowAccessPoints = -10,
            AskForIDPoints = 5,
            DenyAccessPoints = 10
        },
        new EmailScenario
        {
            Content = "Hello, the accounting department needs your login details to verify an error in your last pay stub.",
            AllowAccessPoints = -15,
            AskForIDPoints = 10,
            DenyAccessPoints = 5
        },
        new EmailScenario
        {
            Content = "Hey, I’m from the software team, and we need admin rights to install a new tool for your department.",
            AllowAccessPoints = -20,
            AskForIDPoints = 10,
            DenyAccessPoints = 10
        },
        new EmailScenario
        {
            Content = "I’m working on a project and require access to the database of your past transactions.",
            AllowAccessPoints = -10,
            AskForIDPoints = 5,
            DenyAccessPoints = 10
        },
        new EmailScenario
        {
            Content = "This is the building maintenance team. We need access to the server room to update the fire safety system.",
            AllowAccessPoints = -15,
            AskForIDPoints = 10,
            DenyAccessPoints = 5
        },
        new EmailScenario
        {
            Content = "Hi, I need temporary access to the secure server to fix a critical error for your department.",
            AllowAccessPoints = -10,
            AskForIDPoints = 10,
            DenyAccessPoints = 10
        },
        new EmailScenario
        {
            Content = "I’m from customer support, and there’s a flagged issue with your recent login. Can you share your credentials for verification?",
            AllowAccessPoints = -20,
            AskForIDPoints = 5,
            DenyAccessPoints = 10
        },
        new EmailScenario
        {
            Content = "Hello, I’m conducting an audit of your department’s systems and need access to your files.",
            AllowAccessPoints = -15,
            AskForIDPoints = 10,
            DenyAccessPoints = 5
        },
        new EmailScenario
        {
            Content = "Hey, I’m from marketing. Can you provide access to last quarter’s reports for an upcoming campaign?",
            AllowAccessPoints = -10,
            AskForIDPoints = 10,
            DenyAccessPoints = 5
        },
        new EmailScenario
        {
            Content = "Hi, this is the IT helpdesk. We need to perform a mandatory update on your account. Please provide login credentials.",
            AllowAccessPoints = -20,
            AskForIDPoints = 10,
            DenyAccessPoints = 10
        },
        new EmailScenario
        {
            Content = "Hello, this is the facilities team. We need access to the office layout plans for a fire drill.",
            AllowAccessPoints = -5,
            AskForIDPoints = 20,
            DenyAccessPoints = 0
        },
        new EmailScenario
        {
            Content = "Hi, I’m from IT. There’s a critical security patch. Please send your login details to proceed.",
            AllowAccessPoints = -25,
            AskForIDPoints = 5,
            DenyAccessPoints = 15
        },
        new EmailScenario
        {
            Content = "Hey, the legal department needs access to confidential case files for an audit. Can you help?",
            AllowAccessPoints = -30,
            AskForIDPoints = 15,
            DenyAccessPoints = 5
        },
        new EmailScenario
        {
            Content = "This is HR. We’re updating employee records and need access to your performance review.",
            AllowAccessPoints = -10,
            AskForIDPoints = 10,
            DenyAccessPoints = 20
        },
        new EmailScenario
        {
            Content = "I’m the new IT head. Can you grant me admin access to the employee database?",
            AllowAccessPoints = -50,
            AskForIDPoints = 20,
            DenyAccessPoints = 10
        },
        new EmailScenario
        {
            Content = "Hi, this is finance. We’ve detected a duplicate transaction and need your account access to resolve it.",
            AllowAccessPoints = -40,
            AskForIDPoints = 5,
            DenyAccessPoints = 15
        },
        new EmailScenario
        {
            Content = "Hello, I’m your supervisor’s assistant. We need your access credentials to finalize a report.",
            AllowAccessPoints = -15,
            AskForIDPoints = 10,
            DenyAccessPoints = 5
        },
        new EmailScenario
        {
            Content = "I’m from the CEO’s office. There’s an urgent meeting, and we need access to last quarter’s data.",
            AllowAccessPoints = -20,
            AskForIDPoints = 15,
            DenyAccessPoints = 10
        },
        new EmailScenario
        {
            Content = "This is security. There’s a breach attempt on your account. Confirm your credentials to secure it.",
            AllowAccessPoints = -50,
            AskForIDPoints = 10,
            DenyAccessPoints = 20
        },
        new EmailScenario
        {
            Content = "Hey, I’m from procurement. Can you give me access to the supplier database to confirm an order?",
            AllowAccessPoints = -10,
            AskForIDPoints = 10,
            DenyAccessPoints = 5
        },
        new EmailScenario
        {
            Content = "Hi, I’m from the IT department. We’re performing scheduled maintenance on the server.",
            AllowAccessPoints = 20,
            AskForIDPoints = 10,
            DenyAccessPoints = -10
        },
        new EmailScenario
        {
            Content = "Hello, this is HR. We need access to update your department’s payroll information.",
            AllowAccessPoints = 15,
            AskForIDPoints = 5,
            DenyAccessPoints = -15
        },
        new EmailScenario
        {
            Content = "Hey, this is your manager. I need quick access to the project files for a presentation.",
            AllowAccessPoints = 25,
            AskForIDPoints = 10,
            DenyAccessPoints = -20
        },
        new EmailScenario
        {
            Content = "This is from the logistics team. We need access to confirm a critical shipment.",
            AllowAccessPoints = 10,
            AskForIDPoints = 5,
            DenyAccessPoints = -10
        },
        new EmailScenario
        {
            Content = "Hi, I’m a field technician. Can you give me access to the equipment diagnostics?",
            AllowAccessPoints = 30,
            AskForIDPoints = 15,
            DenyAccessPoints = -25
        },
        new EmailScenario
        {
            Content = "Hello, this is from the finance department. We’re resolving a budget discrepancy.",
            AllowAccessPoints = 15,
            AskForIDPoints = 10,
            DenyAccessPoints = -20
        },
        new EmailScenario
        {
            Content = "I’m from the IT helpdesk. We detected a system issue that requires immediate intervention.",
            AllowAccessPoints = 20,
            AskForIDPoints = 5,
            DenyAccessPoints = -15
        },
        new EmailScenario
        {
            Content = "This is your direct supervisor. I need access to your work logs for performance evaluation.",
            AllowAccessPoints = 25,
            AskForIDPoints = 10,
            DenyAccessPoints = -10
        },
        new EmailScenario
        {
            Content = "Hi, I’m from the audit team. Can you grant temporary access to verify compliance records?",
            AllowAccessPoints = 15,
            AskForIDPoints = 10,
            DenyAccessPoints = -10
        },
        new EmailScenario
        {
            Content = "Hey, I’m from the new software implementation team. We need access to test the system integration.",
            AllowAccessPoints = 20,
            AskForIDPoints = 5,
            DenyAccessPoints = -20
        },
        new EmailScenario
        {
            Content = "Hi, I’m from the marketing team. We need urgent access to the latest campaign analytics.",
            AllowAccessPoints = 15,
            AskForIDPoints = 10,
            DenyAccessPoints = -10
        },
        new EmailScenario
        {
            Content = "This is the facilities department. Can you provide access to confirm the building’s energy usage report?",
            AllowAccessPoints = 20,
            AskForIDPoints = 5,
            DenyAccessPoints = -15
        },
        new EmailScenario
        {
            Content = "Hello, this is customer support. We need temporary access to resolve a major client complaint.",
            AllowAccessPoints = 25,
            AskForIDPoints = 10,
            DenyAccessPoints = -20
        },
        new EmailScenario
        {
            Content = "Hey, this is your project coordinator. Please give me access to the shared design files for a review.",
            AllowAccessPoints = 15,
            AskForIDPoints = 10,
            DenyAccessPoints = -5
        },
        new EmailScenario
        {
            Content = "I’m from the supply chain team. Can you grant access to the supplier database for a reconciliation task?",
            AllowAccessPoints = 20,
            AskForIDPoints = 10,
            DenyAccessPoints = -25
        },
        new EmailScenario
        {
            Content = "Hi, this is the executive team assistant. We need access to quarterly performance reports for a meeting.",
            AllowAccessPoints = 30,
            AskForIDPoints = 15,
            DenyAccessPoints = -15
        },
        new EmailScenario
        {
            Content = "This is the IT department. We need your credentials to troubleshoot connectivity issues with your account.",
            AllowAccessPoints = 25,
            AskForIDPoints = 10,
            DenyAccessPoints = -20
        },
        new EmailScenario
        {
            Content = "Hello, I’m from the compliance team. Grant me access to the project documentation for an internal audit.",
            AllowAccessPoints = 15,
            AskForIDPoints = 5,
            DenyAccessPoints = -10
        },
        new EmailScenario
        {
            Content = "Hey, this is the customer relations team. We need access to order histories to resolve pending cases.",
            AllowAccessPoints = 20,
            AskForIDPoints = 10,
            DenyAccessPoints = -10
        },
        new EmailScenario
        {
            Content = "Hi, this is the events management team. We need access to finalize venue bookings for the annual meeting.",
            AllowAccessPoints = 25,
            AskForIDPoints = 5,
            DenyAccessPoints = -15
        }
    };

    public static EmailScenario GetRandomScenario
        => se_scenarios[Random.Range(0, se_scenarios.Count)];
}
