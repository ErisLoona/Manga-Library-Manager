# A manga downloader and library manager
### [Download .exe](https://github.com/ErisLoona/Manga-Library-Manager/releases)  
This program enables the downloading and better organization of downloaded mangas. The primary supported format is `.epub`, however `.cbz` is also supported in all functions.  
The downloads and sync features rely on the [MangaDex.org](https://mangadex.org/) API.  

Features:  

- Easily download mangas from MangaDex, however you want to  
	- Choose which chapters to download - the program automatically selects each chapter; if a chapter was scanlated by more than one group, the program will try and select the scanlator who covered most of the chapters in the manga to maintain consistency  
		- There is an option to instead prefer the group who covered most chapters *up to each chapter*, to allow consistency in chunks for mangas with spotty coverage  
		- The program also offers a button to quickly unselect / reselect interlude chapters / extras, but be warned this is a "dumb" feature, it (un)selects the chapters with decimals, it has no way of knowing the actual content of the chapter  
	- Choose which title to use - the program lets you choose between the main title and any alt-titles listed on MangaDex; the program tries to get the alt-titles in your preferred language  
	- Downloaded mangas are ready to be read - after downloading, the program creates a single `.epub` file with all the chapters, the cover image, author and artist; alternatively, it can create a single `.cbz`, however this will have no metadata (the cover image is still included, as the first entry)  
	- Data-saver offered - if you are one of the poor souls who still has to deal with a data cap, you are also covered; the program offers to download the mangas in data-saver quality  
- Update the mangas you've already downloaded (*not limited to those downloaded with this program!*) to the latest chapter available on MangaDex  
	- The program will automatically merge the new chapter(s) into your existing file; works with both `.epub` and `.cbz`, but be advised the program will use its own formatting for the table of contents in `.epub` files for the new chapters, so it may look a bit mismatched depending on how you had it before; you can always choose to download the new chapter(s) separately and integrate them yourself with an external program like Calibre  
- Quickly and easily manage your library of downloaded mangas  
	- Add new manga files to your library individually or in bulk  
		- The program will ignore duplicate entries, so you can always "scan" the same one folder and the program will add all the *new* mangas  
	- When adding a new manga to your library the program will try and get its MangaDex link automatically (but sometimes it needs you to select the correct result from the search), but you can always set it yourself  
		- Double-click on the link to open it in your browser  
	- Automatically check and set whether the manga is ongoing or not (ongoing or hiatus are considered ongoing, completed or cancelled are considered not ongoing)  
	- Quickly check all of your ongoing mangas for new chapters at once  
	- Automatically get the content rating  
	- Fully customizable tags system  
		- Automatically get, set and reset the (resetting will also resync the ongoing status) tags used on MangaDex  
		- Add new custom tags  
		- Select or unselect tags however you want  
	- Fast, powerful filtering by tags  
		- Filter mangas by including and excluding any tags  
		- You can both include and exclude tags by `and` and `or` independently (as in include or exclude mangas including or excluding *all* or *any* of the selected tags)  
	- Additionally and independently filter to only show ongoing mangas  
	- Quickly view files in File Explorer to copy or move them  
	- Quickly delete mangas from the program itself; you can choose to only remove the entry or to also delete the file; be warned, it *deletes* the file, not move it to the Recycle Bin  
	- Automatically get the last chapter in the `.epub` file (only with `.epub` files, only with the correct specific formatting of the description of the `.epub`; when updating a `.epub` manga the program will automatically add in the downloaded chapters in the format it's looking for), but you can also set it yourself  
- Portable, and compatible with removable drives  
	- The program can be stored on a removable drive alongside your library, it always tries to make paths to mangas relative to its executable, so if you keep your library "downstream" of the executable you don't have to worry about migrating it entirely, or Windows assigning your removable drive a different letter  
	- Your library entries and settings are saved separately in a `.json` file right alongside the executable, so you can update the `.exe` to the latest version without worry  

For any bugs, questions or.. issues, please [check or open an issue](https://github.com/ErisLoona/Manga-Library-Manager/issues). If you wanna know how a specific feature works in more detail, please [check the wiki](https://github.com/ErisLoona/Manga-Library-Manager/wiki) first.  

The program is written entirely in WinForms, C# .NET 8.0 and is intended for use on Windows. In my (admittedly not very extensive) testing on Linux it seems to work fine through Wine with no additional configuration, but ymmv. Happy to try and help, but definitely no guarantees.  

This is a passion project, but it still took *a lot* of work, so if you could please consider a donation of any amount, everything is appreciated!  
[![ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/N4N0OTIEV)  

## [Credits](https://github.com/ErisLoona/Manga-Library-Manager/wiki/Credits)
