# NFS.Unpacker :see_no_evil:
Tool for extract ZZDATA archives from Need for Speed console games series

### Usage
> NFS.Unpacker "**ZDIR_FORMAT**" "**ZDIR_FILE**" "**UNPACK_DIRECTORY**"

### Examples
> NFS.Unpacker "**-false**"  "**E:\Games\Need For Speed Carbon [GW5E69]\root\NFS\ZDIR.BIN**" "**E:\MOD\UNPACKED**"

> NFS.Unpacker "**-true**"  "**E:\Games\SLUS-20362\NFSHP2\ZDIR.BIN**" "**E:\MOD\UNPACKED**"

> [!important]
> The Projects folder must be in the tool folder, don't forget to copy it 😊

> [!caution]
> Use "**-true**" if you are trying to unpack files from Hot Pursuit 2 (PS2) in other cases "**-false**"

# Tested games
| Game   | Platform   | Old format   |
|--- |---  |--- |
| Need for Speed: Hot Pursuit 2 | PS2 | true |
| Need for Speed: Underground | PS2 | false |
| Need for Speed: Underground | XBOX | false |
| Need for Speed: Underground 2 | PS2 | false |
| Need for Speed: Underground 2 | XBOX | false |
| Need for Speed: Most Wanted | PS2 | false |
| Need for Speed: Most Wanted | XBOX | false |
| Need for Speed: Carbon | GC | false |
| Need for Speed: Carbon | PS2 | false |
| Need for Speed: Carbon | Wii | false |
| Need for Speed: Undercover | PS2 | false |
| Need for Speed: Undercover | XBOX | false |

# NFS.Packer :see_no_evil:
Tool for repack BIN archives from games ☝️

### Usage
> NFS.Packer "**INPUT_DIRECTORY**" "**OUTPUT_DIRECTORY**"

> [!note]
> **INPUT_DIRECTORY**: Directory with unpacked files
> 
> **OUTPUT_DIRECTORY**: Directory for output files (ZDIR and etc.)

### Example
> NFS.Packer "**E:\MOD\UNPACKED**" "**E:\MOD\NEW**"

> [!warning]
> I have not tested the packer in games, but it should work 😅