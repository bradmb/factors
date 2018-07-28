# Factors
*NETStandard 2.0 library that makes it easy to implement multi-factor (and FIDO2) authentication into your application*

Lots to still do on this project, but here's what is built out so far:
* In-memory database (mostly to assist with unit testing)
* Email multi-factor authentication
* Users can have more than one credential (example: three phone numbers, FIDO2 key, and a TOTP token)

To-do:
* SqlServer support
* Text message multi-factor (via Twilio)
* Phone call multi-factor (via Twilio)
* Authy multi-factor
* TOTP multi-factor
* HOTP multi-factor
* FIDO2 support (for WebAuthn)

----------

*Pull requests and issue reports always appreciated*
