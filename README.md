<!-- GRAPHIC -->

# bsn.Har

.NET Library for handling HAR requests and responses.

In addition to serialization and deserialization support, it also has some helper code for creating a HAR-based HTTP server (received HAR requests and responds with HAR responses).

<!-- badges -->

---
## Links

- [HAR 1.2 Spec](http://www.softwareishard.com/blog/har-12-spec/)
- [MFilesExtensionGateway](https://github.com/avonwyss/bsn.MFilesExtensionGateway) (uses bsn.Har to proxy requests just via a single string input/output)

---
## Description

- Implements .NET HAR serialization and deserialization
- Helper methods for getting and setting bodies
- Parsing of query strings and `multipart/form-data` and `application/x-www-form-urlencoded` bodies
- Lightweight framework for routing HAR requests to create a HTTP server, including command-pattern and binding support

<!--
---
## FAQ
- **Q**
    - A
-->

---
## Source

[https://github.com/avonwyss/bsn.Har](https://github.com/avonwyss/bsn.Har)

---
## License

- **[MIT license](LICENSE.txt)**
- Copyright 2022 © Arsène von Wyss.
