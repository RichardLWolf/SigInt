# RTL-SDR Signal Monitor & Playback Viewer

**A Windows-based RTL-SDR tool for detecting, capturing, and reviewing RF signals.**

## Overview
This application continuously **monitors a center frequency**, detecting signals that exceed **15 dB above the noise floor**. When a signal is detected, the system **captures up to 60 seconds of IQ data** and saves it in a **compressed archive (ZIP format)**—ensuring no more than **one capture every 10 minutes**.

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

## 📥 Download
🚀 **[Latest Release & Download EXE](https://github.com/RichardLWolf/SigInt/releases/latest)**

---
