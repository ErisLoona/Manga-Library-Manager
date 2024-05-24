# Manga Library Manager
### [Download .exe](https://github.com/ErisLoona/Manga-Library-Manager/releases/tag/v1.7)
This is a manager for personal use that allows me to organize and keep track of ongoing releases of Mangas. It checks the `content.opf` file inside a `.epub` archive to get the title and last chapter listed in the description of the "book". It also attempts to get the `cover.*` image from the root of the epub archive.<br />

The program enables better organization of an offline digital manga (/book) library. The intended use (and actions of the program) goes as follows:<br />
The user adds a manga, either through the Add Manga button which, while it allows multiple selections it will warn the user of any duplicate entries, or through the Scan Directory button, which will scan the selected directory and all its subdirectories for `.epub` files and add all the new entries without warning the user about duplicates (I added this because I don't want to find the new additions manually, so I just Scan Directory and it adds everything new without extra interaction). The program will add the entries, the paths to the files are stored relative to the executable if possible (this enables the storage of one's library on a removable drive, alongside the executable, thus preventing the paths from breaking if upon plugging the drive in it is given a different drive letter).<br />

Upon selecting a new entry for the first time (triggered by the link of the entry not being set, so one could re-trigger this intentionally) the program will search the MangaDex API for the title of the manga (obtained from `content.opf`, the file name is irrelevant) and returns all the results in order of relevance in the link ComboBox. The program will select the first result automatically, and the user can double-check whether this is the correct result by the title of the entry or by double-clicking the ComboBox which will open the generated link in the default browser. The program composes the link from the API response. From the same API call the program will also make use of and store:<br />

- The Status of the manga - `ongoing` and `hiatus` are considered as ongoing and the checkBox is checked, `completed` and `cancelled` are considered as not ongoing and the checkBox is left unchecked<br />
- The Content Rating of the manga - displayed in the Description panel<br />
- The Tags of the manga - displayed in the Description panel

The program will obtain the last chapter from the `content.opf` as well, this is specific to my personal way of merging the individual chapter epubs in Calibre, namely I always name them as `Ch.XXXX` where the Xs are the number, for example `Ch.001` etc. which are then all listed in the final epub's description. The program uses the Regex string `Ch\.[0-9.]+` to get all the Chapter entries from the epub's description (from `content.opf`) and simply gets the highest number. The manga's cover is obtained from the epub file, specifically in the root of the epub file the program will look for `cover.jp(e)g`, `cover.png` or `cover.webp` (the cover being in the root may be specific to merging epubs in Calibre, ymmv). Failing to find it, the program will display a placeholder error image. Because the cover is always obtained dynamically, the program checks whether the file exists every time an entry is selected from the list. The user can opt to remove it from the list, or to keep it (intended to save the link / custom rating / custom tags before deletion).<br />

Should the user want to for any reason, they can modify the last chapter however they want. The reset button on the right of the numericUpDown allows the user to reset the number to whatever the program gets from the `content.opf` description. Similarly, the link input is more of a suggestion, should the search return no results or should the user want to change the link in any way, that is possible. Ditto with the Ongoing checkmark.<br />
The three buttons in the Description panel:<br />
- The Open in File Explorer button will open File Explorer at the path of the selected entry's file, and select the file - intended to allow quickly interacting with the file without having to find it manually<br />
- The Check Online button will check the MangaDex API using the provided link for the latest English-translated chapter of the manga and display it, alongside a difference between the last chapter listed in the program - this is intended for ongoing mangas, to check whether a new chapter is available / how many chapters ahead is the online publication compared to the user's file<br />
- The Delete Entry button does what it says on the tin, allowing the user to choose between only deleting the entry in the program or *also* deleting the file - the file is immediately deleted, not moved to the Recycle Bin! Please be careful with it

The Edit Tags button allows the user to customize the automatically obtained list of tags, as well as add their own custom tags. The program keeps a list of all the unique tags which is listed in the form, and how many times each appears. As such, if a tag is removed and it was its only (/last) apparition, it will be removed from the list altogether on subsequent openings of the form. Custom user tags must be different from existing tags (quality of life in my opinion, don't want multiple versions of the same tag). The reset button allows the user to reset the tags and content rating (*and Ongoing status*) of the manga from the MangaDex API, intended in case the user messes up the tags or any other reason. No tags will be preserved, including custom ones, and all tags will be removed from the list on subsequent viewings of the form if it was their only / last apparition.<br />

There are two filtering systems in the program, which work together. The first one (third top button) will toggle between showing all mangas, and only showing Ongoing mangas. This is useful to know which ones should be "checked online" for a new chapter. The other one allows the user to Filter by Tags. The user can select which Content Ratings to include, and which tags to include / exclude and how. Inclusion and exclusion modes work as follows: `and` will only *include* mangas that contain **all** the selected tags, and will only *exclude* mangas that contain **all** the selected tags. `any` will *include* mangas that contain **any** of the selected tags, and will *exclude* mangas that contain **any** of the selected tags. These modes can be mixed and matched however the user wants. Unselected tags will be ignored (i.e. it doesn't matter whether a manga has them or not). The form includes the total number of entries, number of entries for each Content Rating (unknown / not set are not included), a sorted list of all the present tags with their respective number of entries, and buttons to allow for quickly resetting the filters.<br />

The Check All Online button will go through each Ongoing manga and retrieve its latest chapter, then display a sorted list of every manga and whether or not there are new chapters available, and if yes how many. It gives the same information as if the user were to click on the Check Online button, but it's done in bulk for convenience.<br />

The program will only save the entries database upon exiting. Should it fail to write the `.json` for any reason, the user will be given the option to get the json string themselves to do with as they please.
