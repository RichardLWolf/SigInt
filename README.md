# RTL-SDR Signal Monitor & Playback Viewer

**A Windows-based RTL-SDR tool for detecting, capturing, and reviewing RF signals.**

## Overview
This application continuously **monitors a center frequency**, detecting signals that exceed **15 dB above the noise floor**. When a signal is detected, the system **captures up to 60 seconds of IQ data** and saves it in a **compressed archive (ZIP format)**—ensuring a minimum delay between captures, as configured by the user.

The **Playback Viewer** allows users to:
- **Review captured signals** via waveform and waterfall visualizations.
- **Scrub through recorded IQ data** with a seek bar.
- **Export recordings to SDRSharp-compatible WAV format** for further analysis.

## Key Features
📡 **Real-Time Signal Detection** – Automatically records signals that exceed a configurable threshold.  
💾 **Compressed IQ Data Storage** – Saves signal captures efficiently in a ZIP archive.  
📊 **Playback & Visualization** – Displays waveform and waterfall plots of recorded signals.  
⏪ **Seekable Playback** – Scrub through recordings with a timeline control.  
🎛 **WAV Export for SDRSharp** – Convert recorded IQ data into a Baseband WAV file for external analysis.  

## Use Case
Ideal for **radio enthusiasts, spectrum monitoring, and signal intelligence (SIGINT) applications**, this tool enables efficient detection, recording, and review of intermittent RF signals.

## Screenshots
![SigInt Main Window](https://raw.githubusercontent.com/RichardLWolf/SigInt/master/main_screen.png)

![Playback Window](https://raw.githubusercontent.com/RichardLWolf/SigInt/master/playback.png)

## 📥 Download
🚀 **[Latest Release & Download EXE](https://github.com/RichardLWolf/SigInt/releases/latest)**

---
# Getting Started with SigInt  

Don't have an RTL-SDR and want to get started? This guide will walk you through setting up an **RTL-SDR** device and configuring **SigInt** to begin capturing signals.

## 1. Get an RTL-SDR Device  
You'll need a **USB RTL-SDR receiver**. The [**RTL-SDR Blog V3**](https://www.google.com/search?q=V3+R860+RTL2832U+1PPM+TCXO+HF+Bias+Tee+SMA+Software+Defined+Radio+with+Dipole+Antenna+Kit)
 is recommended and widely supported. Other RTL-SDR dongles may also work but have not been tested with SigInt.

### **Connect an Antenna**  
After plugging in your RTL-SDR, attach an **antenna** for best reception.  
For example, for signals near **1.6 GHz**, the ideal antenna length depends on the type:

- **Half-wave dipole:** **3.5-inch elements per side** (best for standalone dipole use).  
- **Quarter-wave monopole:** **1.75-1.8-inch elements** (requires a ground plane).  

A common, inexpensive, and effective choice is a **dipole antenna with 3.5-inch arms**, as shown below:  

![Dipole Antenna](https://raw.githubusercontent.com/RichardLWolf/SigInt/master/antenna.png)  

(Adjustable dipole kits work well if you need to experiment with different lengths.)

## 2. Install RTL-SDR Drivers  
Before using your device, follow the official **RTL-SDR Blog’s Getting Started Guide**:  
👉 [RTL-SDR Blog – Start Here](https://www.rtl-sdr.com/rtl-sdr-quick-start-guide/)  

This will walk you through:
- Installing **Zadig** to replace the default Windows driver.
- Verifying that the device is detected properly.

## 3. Install SDR# (SDRSharp)  
To confirm your RTL-SDR is working, install **SDR#** (pronounced "S-D-R Sharp"). It is a **free** software-defined radio tool for personal and non-commercial use:  
👉 [Download SDR#](https://airspy.com/download/)  

1. Choose the **"Software Defined Radio Package"** link on the right side.  
2. Run the **install-rtlsdr.bat** script inside the folder.  
3. Open **SDRSharp.exe**, select **RTL-SDR (USB)** as the source, and press **Start**.  
4. Tune to a known local FM station (~88-108 MHz) using the **big numbers at the top of the window** to confirm reception.  

If you hear audio and see a signal, your RTL-SDR is working.

## 4. Install and Configure SigInt  
Once your RTL-SDR is working, download and install **SigInt** from the repository:  
👉 [Latest SigInt Release](https://github.com/RichardLWolf/SigInt/releases/latest)

1. **Plug in your RTL-SDR device** before launching SigInt.  
2. Select the correct SDR device from the dropdown.  
3. Press the **green play button** (![Play Button](https://raw.githubusercontent.com/RichardLWolf/SigInt/master/My%20%Project/Resources/media_play.png)) to begin monitoring.  
4. The app defaults to **1.6 GHz**. If you need a different frequency, click the **gear button** (![Gear Button](https://raw.githubusercontent.com/RichardLWolf/SigInt/master/My%20%Project/Resources/gear.png)) to adjust it.  

### **How SigInt Works**
- The signal spectrum will be displayed, and monitoring will begin.  
- **Minimize SigInt** and let it run. It will automatically detect and log signal events.  
- To view the log, click the **green folder icon** to open the log folder, then double-click the `"Application_Log.txt"` file in File Explorer. 
- To view recorded events, click the **microphone icon** to open the playback window.  Select the desired archive from the list at top-left then click the play button to begin playback.
---
