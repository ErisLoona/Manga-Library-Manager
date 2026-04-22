# Manga Library Manager

A powerful manga downloader and offline library management tool built for ease of use and organization.  

### [**Download Latest Release**](https://github.com/ErisLoona/Manga-Library-Manager/releases)  

-----

## Overview

This program streamlines the process of downloading, updating, and organizing your offline manga collection. The downloads and sync features rely on the [MangaDex.org](https://mangadex.org/) API.  

  * **Supported Formats:** `.epub` (full metadata support) and `.cbz` (basic support)  
  * **Compatibility:** Cross-platform support for Windows, macOS, and Linux  
  * Built with C# .NET 10.0 and [AvaloniaUI](https://github.com/AvaloniaUI/Avalonia)  

-----

## Features

### 1\. Smart Downloading

  * **Sequential Queuing:** Add multiple series to a queue and let the program download them one after another  
  * **Automatic Chapter Selection:** Automatically selects chapters based on the scanlator with the most consistent coverage  
    * Any chapters can be selected / unselected independently  
  * **Flexible Titling:** Choose between the main title or localized alternative titles provided by MangaDex  
  * **Formatted Output:** Creates a single file containing all chapters, cover art, and metadata (Author/Artist)  
  * **Localization Support:** Can download any language translation available on MangaDex  
  * **Data Saver Mode:** Optional low-quality image downloads for users with limited data or storage  

### 2\. Library Management & Updates

  * **Universal Updating:** Update any manga in your offline library to the latest chapter, even if it wasn't originally downloaded with this tool  
  * **Automatic Merging:** Integrates new chapters into your existing `.epub` or `.cbz` files  
  * **Bulk Importing:** Add files individually or in bulk; the program automatically ignores duplicates  
  * **Metadata Sync:** Quickly refresh covers, descriptions, publication status and content ratings directly from MangaDex  
  * **File Operations:** Open file locations in Explorer or delete entries and / or files directly  

### 3\. Advanced Tagging and Filtering

  * **Custom Tag System:** Support for MangaDex tags and user-defined custom tags  
  * **Complex Filtering:** Filter your offline library using `AND` / `OR` logic for both inclusions and exclusions  
  * **Status Tracking:** Quickly filter for "Ongoing" series to check for new releases  

### 4\. Portability

  * **Relative Pathing:** Ideal for use on removable drives. As long as your offline library is stored in the same directory (or a sub-directory) as the executable, file paths remain valid across different computers  
  * **Self-Contained Settings:** All offline library data and configurations are stored in a local `.json` file, which can be hidden for a neater library appearance  
  * **Optional Update Checks:** If enabled, will give a notification when there is a new program version available  

-----

## Support and Resources

  * [Open an Issue](https://github.com/ErisLoona/Manga-Library-Manager/issues)  
  * [Credits](https://github.com/ErisLoona/Manga-Library-Manager/wiki/Credits)  

If you find this project useful, please consider supporting its development:  

[![ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/N4N0OTIEV)