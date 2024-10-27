# VariableBPM Extension for Vegas Pro
## Introduction
![速](https://github.com/user-attachments/assets/beb018ee-1c62-41de-874d-d9a94740ae0f)
VEGAS Pro is known as a video software that's good at editing audio. However, to this day, VEGAS Pro still does **NOT** support entering variable BPM values and letting the grid change with them. This extension appears to solve this puzzle.

**With this extension, you can now label tempo values in your projects and display tempo changes in real time on the grid in VEGAS Pro.**

![image](https://github.com/user-attachments/assets/09dda1b3-8987-4608-8d10-22e184e3a5f0)

<br>

### Install
Go to [Latest Releases](https://github.com/zzzzzz9125/Miscz/releases/) to download the .zip file. Unzip and place all the files inside it into the extension folder. `VariableBpm_13.dll` is for Sony Vegas Pro 13, and `VariableBpm.dll` is for Magix Vegas Pro 14+.

**The most recommended path is this one, which allows both Sony and Magix versions to use this extension:** `C:\ProgramData\Sony\VEGAS Pro\Application Extensions\`

<br>

Here're all the available paths: (17.0 = your Vegas Pro version number)

**`C:\ProgramData\VEGAS Pro\Application Extensions\`**

`C:\ProgramData\VEGAS Pro\17.0\Application Extensions\`

`%userprofile%\Documents\Vegas Application Extensions\`

`%appdata%\VEGAS Pro\Application Extensions\`

`%appdata%\VEGAS Pro\17.0\Application Extensions\`

`%localappdata%\VEGAS Pro\Application Extensions\`

`%localappdata%\VEGAS Pro\17.0\Application Extensions\`

**For Sony, it should be: `...\Sony\VEGAS Pro\...`**

<br>

If no folder exists, create one, then put the files in.

<br>

### How To Use
**Note: The following is translated from [the Chinese document](https://www.bilibili.com/read/cv39509770/) I wrote.**

Let's start with the logic of how it works. The extension is based on the recognition of the markers in VEGAS Pro, and for the markers that meet the format requirements, the extension records them as BPM tempo markers. When `Auto Refresh` is enabled and the cursor position is anywhere in the project, the extension will refresh the tempo parameters of the current project and align the timeline to form a continuous and changeable BPM grid.

**When the cursor position changes, the parameters here will be automatically refreshed:**

![image](https://github.com/user-attachments/assets/32f9530d-ad26-4e1d-ab9c-11892bc974a1)

**The marker label format is self-created, such as the following content can be recognized by the extension (case insensitive): `BPM128` `114BPM +5.1` `BPM 191 - 9.8.10` `BPM81 RESET` `BPM = 192, RESET, BEATS = 3`**

You can see that the format is very free, and basically you can write it any way you want.

<br>

**BPM markers are designed to record changes, that is, how the current marker has changed compared to previous parameters, a bit like tempo change in MIDI. There're several parameters:**

**`Marker Position`**: Determined by the position of the marker on the timeline.

<br>

**`BPM`**: Represents the BPM value from the current time, which can be any positive number. Note that in order for the marker to be recognized by the extension, the marker label should contain:
1. the keyword **`BPM`**
2. one BPM value that can be recognized
3. only content that the extension can recognize

<br>

**`RESET`**: Resets the timecode of the current time to `1.1.000`, and affects the timecode after it.

![image](https://github.com/user-attachments/assets/c75a49d2-8a5b-408f-98f3-be996a1996a2)![image](https://github.com/user-attachments/assets/23525897-5946-4657-8ddb-c2332f7e311b)

<br>

**`OFFSET`**: Indicates the offset of the timecode value from the current time, and affects the timecode after it. The parameter format is `± Measure.Beat.SmallBeat`. It also supports only `+1` for only `Measure` or `-1.2` omitting `SmallBeat`. The RESET parameter will also reset the offset accumulated before the current time. For example, parameter `RESET + 1.2` will set the current timecode to `1.1.000 + 1.2.000 = 2.3.000`.

![image](https://github.com/user-attachments/assets/bf7b46e3-98b3-4039-818d-2c2087202084)

<br>

**`BEATS`**: Indicates the count of beats in a measure, such as `BEATS = 3` to change the BPM grid to 3 beats from the current time. (To avoid confusion with the BPM value, it must be preceded by `BEATS=`, and it doesn't matter if there's a space before or after `=`.) For the timecode conversion of the variable beats is not very easy, the timecode before and after the switch of beats will be discontinuous. If you mind, you can use `RESET` to reset to `1.1.000`.

<br>

**Then there're the additional features, as shown below:**

![image](https://github.com/user-attachments/assets/544f5b9a-67c3-4737-ae18-8dc12b551e91)

<br>

**Auto Refresh**: Whether to enable the auto refresh for the BPM grid. When it's not enabled, you can also use **Manual Refresh**.

Note that if you really want to just use **Manual Refresh**, you can assign a shortcut to it in *Customize Keyboard*:

![image](https://github.com/user-attachments/assets/653471ce-f020-474b-b722-794a8eb02fb7)

<br>

**Metronome**: A metronome that can be beat manually to calculate BPM. Using the mouse wheel over the textbox, you can quickly multiply and divide the resulting measurement by 2.

<br>

**Tempo Markers Import/Export**: Tempo markers can be import from / export to MIDI files. If MIDI compatibility problems occur, you can try to enable **`MIDI Max Compatibility Mode`** in Settings and import again.

<br>

**Detect Logic**: The detection logic for cursor position of Auto Refresh: **`Timer`**/**`CursorChanged Delegate`**. **`CursorChanged Delegate` is only supported by VEGAS Pro 19+, which is recommended to avoid possible performance issues with `Timer`.** Timer Interval is the detection interval for **`Timer`** and defaults to 1 ms.

<br>

**Markers Ripple**: When a marker is moved, all markers after it can be uniformly moved. **`Only BPM Markers`** moves only the BPM markers, ignoring any other markers that the user has made. **`All Types of Markers`** will move all types of markers, whether they are BPM markers or not.
