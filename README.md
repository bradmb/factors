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

## How It Works

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

### ApplicationName
Just the name of the application you are using Factors in, and may be used in various places within Factors (ex: when sending out a SMS)

### StorageDatabase
This is where all of the Factors data gets stored. Out of the box, Factors supports ServiceStack.OrmLite, which means you get native support for all database providers supported by their library. All you have to do is simply install the associated NuGet package for the database type you want and pass the database provider into the parameter.

Factors is designed to automatically create all tables and indexes it needs, so all you need to do is tell it what database and account to use.

### EncryptionProvider
Currently, all this does is provide hashing support to Factors so it can hash the verification code in the database. If you do not wish to use any encryption for your database, you can simply use the PlainText encryption provider.

### TokenProvider
There's many ways to generate tokens, and the provider you pass here determines what you want your tokens to look like. You can pick from numbers, to letters, or even alphanumeric tokens. You can also configure the length of the token, though they generally default to a six-character token.

## Enabling A Feature
Without a feature, Factors is pretty useless. Features are what make it functional, so let's go over each feature and how to initalize it.

### Email Feature
This allows you to send out your tokens via email, and supports both Postmark and SMTP out of the box.

(TO BE CONTINUED...)

*Pull requests and issue reports always appreciated*
