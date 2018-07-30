# Factors
[![Build Status](https://travis-ci.org/bradmb/factors.svg?branch=master)](https://travis-ci.org/bradmb/factors)

*NETStandard 2.0 library that makes it easy to implement multi-factor (and FIDO2) authentication into your application*

Lots to still do on this project, but here's what is built out so far:
* In-memory database (mostly to assist with unit testing)
* Users can have more than one credential (example: three phone numbers, FIDO2 key, and a TOTP token)
* Numerical multi-factor tokens
* Modular so you only have to install the features you want

In Progress:
* Email multi-factor authentication

To-do:
* SqlServer support
* Text-based multi-factor tokens
* Alphanumeric multi-factor tokens
* Database encryption of secrets
* Text message multi-factor (via Twilio)
* Phone call multi-factor (via Twilio)
* Authy multi-factor
* TOTP multi-factor
* HOTP multi-factor
* FIDO2 support (for WebAuthn)

----------

*Pull requests and issue reports always appreciated*
