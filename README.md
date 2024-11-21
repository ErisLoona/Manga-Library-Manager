# Development branch for migration to AvaloniaUI  
I've decided to re-write the entire program in AvaloniaUI instead of WinForms to enable native cross-platform compatibility. This will take a while, but it will yield a better program. I am also re-writing some parts entirely to make it more readable and/or generally better.  

## TODO list:  
- (Re)Create all the required windows' UI  
	- ~~Main menu~~  
	- Downloader  
	- Filtering  
	- ~~All online chapters~~  
	- ~~Edit metadata~~  
	- ~~Settings~~  
	- ~~Dump JSON and Import library~~ will not be brought over, but the functionality has been brought over (mostly)  
- (Re)Create their functionality  
	- Main menu  
		- Download button  
		- ~~Add manga from file button~~  
		- ~~Check for new chapters button~~  
		- Sorting and filtering button  
		- ~~Settings button~~  
		- ~~Search bar~~  
		- ~~Manga list~~  
		- Manga description panel  
			- ~~Title~~  
			- ~~Cover image (to be extracted async)~~  
			- ~~Labels~~  
			- ~~Open in explorer button~~  
			- ~~Check online button~~  
			- ~~Delete entry button~~  
			- Download new chapters button  
			- ~~Include in bulk checks checkbox~~  
			- ~~Edit metadata button (previously edit tags)~~  
	- Downloader  
		- **TODO** I have not yet settled on a UI or what I want in it  
	- Filtering  
		- **TODO** I have not yet settled on a UI or what I want in it, but I have a good idea  
	- ~~All online chapters~~  
	- ~~Edit metadata~~  
		- Redesigned the UI completely, it looks better and can do more than before including updating the cover file  
	- ~~Settings~~  
- Look into how, if possible, to make this a portable file; I suspect this will pose some issues, but that's a bridge I'll cross when I get there  
- Prettify!  

The MangaDex Library is also written by me, it gets any data I could possibly need from the API in a much neater manner.