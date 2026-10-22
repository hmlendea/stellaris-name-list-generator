# Privacy and Personal Data

This document describes how Stellaris Name List Generator at https://github.com/hmlendea/stellaris-name-list-generator handles personal data. It covers the application behaviour and verified integrations described below. Where the software is self-hosted, the instance operator may have separate responsibilities described below.

## 📑 Table of Contents

- [What This Document Covers](#what-this-document-covers)
- [Self-Hosted Deployments](#self-hosted-deployments)
- [Data We Handle](#data-we-handle)
  - [Data Provided to the Application](#data-provided-to-the-application)
  - [Data Generated or Collected by the Application](#data-generated-or-collected-by-the-application)
  - [Data Received from Integrations](#data-received-from-integrations)
- [Processing and Use](#processing-and-use)
- [Storage, Retention, and Deletion](#storage-retention-and-deletion)
- [External Processing and Integrations](#external-processing-and-integrations)
- [User Controls and Requests](#user-controls-and-requests)
- [International Transfers](#international-transfers)
- [Data Protection and Security](#data-protection-and-security)
- [Document Changes](#document-changes)
- [Contact](#contact)

## 🔎 What This Document Covers

This document describes how Stellaris Name List Generator at https://github.com/hmlendea/stellaris-name-list-generator handles personal data. It covers the application behaviour and verified integrations described below. Where the software is self-hosted, the instance operator may have separate responsibilities described below.

## 🏠 Self-Hosted Deployments

Stellaris Name List Generator is a command-line application that users download, build, and execute on their own machines. The project maintainers do not operate a hosted service. Each operator controls their instance's configuration, local storage, logs, backups, access controls, retention, and request handling.

The application may send data to external services only when the operator explicitly provides URLs in the input XML for optional name-group downloads. Operators can disable this flow by omitting URLs from the input XML or by running the application in an environment without network access.

## 📥 Data We Handle

### Data Provided to the Application

- XML input files supplied by the operator, containing structured name-list records (names, categories, optional URLs). No personal data is requested by the application.

### Data Generated or Collected by the Application

- Local cache files in a `.cache` directory storing downloaded content from user-provided URLs (expires after 8 hours).
- Generated Stellaris name-list text files written to the operator-specified output path.
- No personal data is generated or collected automatically.

### Data Received from Integrations

- Content downloaded from URLs explicitly included in the input XML by the operator. No personal data is received from integrations or third parties unless the operator configures such URLs.

## 🧭 Processing and Use

The application processes the data described above for these verified functions:
- Reading and merging XML name-list records — XML input data
- Downloading and caching optional name-group content from user-provided URLs — URL content
- Generating Stellaris-compatible text output — merged name-list data

## 🗄️ Storage, Retention, and Deletion

- XML input files: stored at operator-chosen paths; retention and deletion controlled by the operator.
- Cache files: stored in a local `.cache` directory relative to the working directory; automatically expire after 8 hours; operator may delete the directory at any time.
- Output files: written to the operator-specified path; retention and deletion controlled by the operator.
- The project does not control storage, deletion, or backups for self-hosted deployments.

## 🔗 External Processing and Integrations

The application has no built-in external data transfer. The only optional network activity is downloading content from URLs explicitly provided by the operator in the input XML.

| Service or integration | Purpose | Data involved | Configuration or documentation |
|-----------------------|---------|---------------|--------------------------------|
| User-provided HTTP/HTTPS URLs | Download optional name-group XML content | XML content from the URL | Input XML `NameGroup` elements with `Url` attribute |

## ⚙️ User Controls and Requests

The following documented controls or request procedures are available:
- Omit `Url` attributes from input XML to disable all network access — edit the input XML file.
- Delete the `.cache` directory to remove all cached downloads — remove the directory in the working folder.
- Specify output path via `--output` CLI argument — control where generated files are written.

## 🌍 International Transfers

Data may be transferred across national or regional borders only when the operator includes URLs pointing to servers in other jurisdictions. The deployment location and any configured URLs are controlled by the instance operator.

## 🛡️ Data Protection and Security

- All file operations are local to the operator's machine.
- Network downloads use HTTPS when the provided URL specifies it; the application does not enforce TLS.
- No secrets, credentials, or authentication tokens are processed or stored.
- The operator is responsible for updates, access controls, backups, network exposure, and log protection on their machine.
- No absolute security is promised.

## 🔄 Document Changes

Update this document when application data flows, storage, integrations, or deployment responsibilities change. The current version is published at https://github.com/hmlendea/stellaris-name-list-generator/blob/master/PRIVACY.md.

## 📬 Contact

For questions about application data handling, contact the project maintainers via GitHub Issues at https://github.com/hmlendea/stellaris-name-list-generator/issues. For a self-hosted instance, contact the instance operator, unless the project explicitly handles the request. Include the repository URL and a description of the data flow in question; do not send passwords, access tokens, or other secrets.