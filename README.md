# A manga downloader and library manager
### [Downloads](https://github.com/ErisLoona/Manga-Library-Manager/releases)  
This program enables the downloading and better organization of downloaded mangas. The primary supported format is `.epub`, however `.cbz` is also supported in all functions.  
The downloads and sync features rely on the [MangaDex.org](https://mangadex.org/) API.  

Features:  

- Easily download mangas from MangaDex, however you want to  
	- Queue up mangas to download or update, the program will download all of them sequentially  
	- Choose which chapters to download - the program automatically selects each chapter; if a chapter was scanlated by more than one group, the program will try and select the scanlator who covered most of the chapters in the manga to maintain consistency  
	- Choose which title to use - the program lets you choose between the main title and any alt-titles listed on MangaDex; the program tries to get the alt-titles in your preferred language  
	- Downloaded mangas are ready to be read - after downloading, the program creates a single `.epub` file with all the chapters, the cover image, author and artist; alternatively, it can create a single `.cbz`, however this will have no metadata (the cover image is still included, as the first entry)  
	- Data-saver offered - if you are one of the poor souls who still has to deal with a data cap, you are also covered; the program offers to download the mangas in data-saver quality  
- Update the mangas you've already downloaded (*not limited to those downloaded with this program!*) to the latest chapter available on MangaDex  
	- You can also update the cover image and title to a new one  
	- The program will automatically merge the new chapter(s) into your existing file; works with both `.epub` and `.cbz`, but be advised the program will use its own formatting for the table of contents in `.epub` files for the new chapters, so it may look a bit mismatched depending on how you had it before; you can always choose to download the new chapter(s) separately and integrate them yourself with an external program like Calibre  
- Quickly and easily manage your library of downloaded mangas  
	- Add new manga files to your library individually or in bulk  
		- The program will ignore duplicate entries, so you can always select all the files in a folder and the program will add all the *new* mangas  
	- Quickly check all of your ongoing mangas for new chapters at once  
	- Automatically get the latest cover, description, ongoing status, content rating and tags from MangaDex  
	- Fully customizable tags system  
		- Automatically get, set and reset the tags used on MangaDex  
		- Add new custom tags  
		- Select or unselect tags however you want  
	- Fast, powerful filtering by tags  
		- Filter mangas by including and excluding any tags  
		- You can both include and exclude tags by `and` and `or` independently (as in include or exclude mangas including or excluding *all* or *any* of the selected tags)  
	- Additionally and independently filter to only show ongoing mangas  
	- Quickly view files in File Explorer to copy or move them  
	- Quickly delete mangas from the program itself; you can choose to only remove the entry or to also delete the file; be warned, it *deletes* the file, not move it to the Recycle Bin  
	- Automatically try to get the last chapter in the `.epub` file (only with `.epub` files, only with the correct specific formatting of the description of the `.epub`; when updating a `.epub` manga the program will automatically add in the downloaded chapters in the format it's looking for), but you can also set it yourself  
- Portable, and compatible with removable drives  
	- The program can be stored on a removable drive alongside your library, it always tries to make paths to mangas relative to its executable, so if you keep your library "downstream" of the executable you don't have to worry about migrating it entirely, or Windows assigning your removable drive a different letter  
	- Your library entries and settings are saved separately in a `.json` file right alongside the executable, so you can update the program to the latest version without worry  
	- The library `.json` can be hidden for a neater appearance of just the executable  
	- The program can **optionally** check for new versions  

For any bugs, questions or.. issues, please [check or open an issue](https://github.com/ErisLoona/Manga-Library-Manager/issues). If you wanna know how a specific feature works in more detail, please [check the wiki](https://github.com/ErisLoona/Manga-Library-Manager/wiki) first.  

The program is written entirely in AvaloniaUI, C# .NET 8.0 and is available for all three major desktop OSes.  

This is a passion project, but it still took *a lot* of work, so if you could please consider a donation of any amount, everything is appreciated!  
[![ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/N4N0OTIEV)  

## [Credits](https://github.com/ErisLoona/Manga-Library-Manager/wiki/Credits)
