# Factors
[![Build Status](https://travis-ci.org/bradmb/factors.svg?branch=master)](https://travis-ci.org/bradmb/factors)

*NETStandard 2.0 library that makes it easy to implement multi-factor (and FIDO2) authentication into your application*

Lots to still do on this project, but here's what is built out so far:
* Support for multiple database types (via ServiceStack.OrmLite)
* Users can have more than one credential (example: three phone numbers, FIDO2 key, and a TOTP token)
* Database one-way hashing of tokens
* Numerical multi-factor tokens
* Text-based multi-factor tokens
* Alphanumeric multi-factor tokens
* Modular so you only have to install the features you want
* Email multi-factor authentication (with support for SMTP and Postmark as mail providers)
* SMS and Phone Call multi-factor (with support for Twilio as a provider)

To-do:
* Authy multi-factor
* TOTP multi-factor
* HOTP multi-factor
* FIDO2 support (for WebAuthn)

----------

# How It Works

Factors is a fully modular library, meaning you can use whatever components you want, or you can even write your own components to use on top of it (ex: using a different email provider's library).

When you initialize Factors, you'll start out by setting up the base system, which requires the following providers:
* Storage Provider
* Encryption Provider
* Token Provider

In the example below, we're going to initalize the main component, and add in our storage, encryption, and token providers:

```
Factors.Initalize(new Models.FactorsConfiguration
{
    ApplicationName = "Factors Unit Test",
    StorageDatabase = new Database.OrmLite.Provider("sql-connection-string", SqlServerDialect.Provider),
    EncryptionProvider = new Encryption.BCryptStandard.Provider(),
    TokenProvider = new Token.Number.Provider()
});
```

Let's cover how each of these properties and the features supported out of the box for them work.

#### ApplicationName
Just the name of the application you are using Factors in, and may be used in various places within Factors (ex: when sending out a SMS)

#### StorageDatabase
This is where all of the Factors data gets stored. Out of the box, Factors supports ServiceStack.OrmLite, which means you get native support for all database providers supported by their library. All you have to do is simply install the associated NuGet package for the database type you want and pass the database provider into the parameter.

Factors is designed to automatically create all tables and indexes it needs, so all you need to do is tell it what database and account to use.

#### EncryptionProvider
Currently, all this does is provide hashing support to Factors so it can hash the verification code in the database. If you do not wish to use any encryption for your database, you can simply use the PlainText encryption provider.

#### TokenProvider
There's many ways to generate tokens, and the provider you pass here determines what you want your tokens to look like. You can pick from numbers, to letters, or even alphanumeric tokens. You can also configure the length of the token, though they generally default to a six-character token.

# Enabling A Feature
Without a feature, Factors is pretty useless. Features are what make it functional, so let's go over each feature and how to initalize it.

## Email Feature
This allows you to send out your tokens via email, and supports both Postmark and SMTP out of the box.

To initialize this feature, simply chain it's method at the end of the `Factors.Initalize` call, like so:

```
Factors.Initalize(new Models.FactorsConfiguration
{
   ...
}).UseEmailFactor(new EmailConfiguration
{
    FromAddress = "name@domain.tld",
    FromName = "My Application Name",
    MailProvider = new Feature.Email.Smtp.EmailSmtpProvider("smtp-host", port: 25, useSSL: false),
    TokenExpirationTime = TimeSpan.FromMinutes(5)
});
```

Now let's go over each of these properties:

#### FromAddress
The email address that you're sending from

#### FromName
The display name that shows who the email is coming from

#### MailProvider
What outbound email transport provider you'll be using. Out of the box you can use both SMTP or Postmark for email delivery, depending on your preference

#### TokenExpirationTime
This is how long the generated token will be valid for. You won't want the tokens to be valid forever, so they will expire after a certain length of time (determined by your entry in this field). Five minutes is usually enough time for a user to receive and enter their token, so it's the suggested minimum value.

### Adding A New Email Credential
When adding a new credential (for any feature), Factors will require that credential to be verified before it's in the "authorized" list of verified credentials. So when you add a new credential, Factors will send out a verification token (if applicable) to the service you're requesting a new credential for.

To add a new credential, simply call the following method:
```
var emailCredential = Factors.ForUser("my-user-account-name").CreateCredential<EmailFeatureType>("name@domain.tld");
```

Now, let's go over each piece here and explain it in more detail.

First, we have `Factors.ForUser("my-user-account-nane")`. Factors keeps track of your users and all of the credentials associated with them, so you will pass a unique id associated with that user (their email address, database id, etc) in the `ForUser()` method so Factors knows who we're managing credentials for.

Next, we have `.CreateCredential<EmailFeatureType>`. This tells Factor what feature we're wanting to interact with. Every feature has it's own type object, and Factors uses this to not only interact with that feature, but also ensure that the data we are storing in the database is associated with the feature we're using.

Finally, there's `"name@domain.tld"`. This is simply the credential we're adding to Factors that we'd like to use two-factor authentication with.

When `Factors.ForUser().CreateCredential()` returns, it will return an object that let's you know if the creation was a success, a `TokenRequestId`, and other information associated with the credential and the token that was generated to verify this new credential. Please note that `TokenRequestId` is important and a value your application needs to use when verifying tokens.

At this stage, an email would have been sent to your user, providing them with their token value that you're application is expecting them to provide.

When your user provides the token to the application and you pass it back to Factors for validation, you simply call the following method:

```
var verificationResult = Factors.ForUser("my-user-account-name").VerifyToken<EmailFeatureType>("token-id", "token-value");
```

Let's cover the pieces that are new here. We'll skip over `Factors.ForUser()` and jump into the new parts.

First, we have `.VerifyToken<EmailFeatureType>`. This is the method you will call when you're verifying a token's value that the user has provided to you. As you can see, we're passing `EmailFeatureType` here, pretty much exactly how we did for `CreateCredential()` above, letting Factors know that we want to validate a token generated for an email address credential.

Next, we have the two parameters. The first parameter is `"token-id"`, which is the `TokenRequestId` that you received when you called `CreateCredential()`. You need to pass this identifier so Factors knows which token to verify against in the database, because in theory you could have multiple verification requests in progress at the same time for the same user.

The second parameter is `"token-value"`, which is the actual token that the user typed into your application and will be checked against the database to determine if it's a match (either by verifying the hashed value in the database if you have that enabled or by doing a simple comparison if you don't have the encryption module installed)

When `Factors.ForUser().VerifyToken()` returns, it will return an object that lets you know if the verification request was completed.

### Phone Feature
This allows you to send out your tokens via text message (SMS) or voice call, and currently supports Twilio out of the box.

To initialize this feature, simply chain it's method at the end of the `Factors.Initalize` call, like so:

```
Factors.Initalize(new Models.FactorsConfiguration
{
   ...
}).UsePhoneFactor(new PhoneConfiguration
{
    EnablePhoneCallSupport = true,
    PhoneCallInboundEndpoint = new Uri("https://my.domain.tld/api/phoneCallReceived,
    MessagingProvider = new PhoneTwilioProvider("account-sid", "auth-token", "phone-number"),
    TokenExpirationTime = TimeSpan.FromMinutes(5)
});
```

Now let's go over each of these properties:

#### EnablePhoneCallSupport
Allows you to have the option for making phone calls that provide the user their Factors token. This is disabled by default, but can be enabled by setting this property to `true` Please note that, if you do enable this feature, you will need to create an API endpoint in your web application that can receive and respond to a request from the messaging service (Twilio, in this example).

#### PhoneCallInboundEndpoint
Only required to be filled out if you set `EnablePhoneCallSupport` to `true`. This is the API endpoint in your web application that will receive a request from the external messaging service (Twilio, in this example), and provide the text for that service to read aloud on the voice call.

#### MessagingProvider
This is the external messaging provider that your text messages (SMS) or voice call will go out through. Only Twilio is currently supported out of the box at this time.

#### TokenExpirationTime
This is how long the generated token will be valid for. You won't want the tokens to be valid forever, so they will expire after a certain length of time (determined by your entry in this field). Five minutes is usually enough time for a user to receive and enter their token, so it's the suggested minimum value.

*Pull requests and issue reports always appreciated*
