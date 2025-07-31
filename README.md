# PC Configurator (.NET MAUI)

📱 A cross-platform mobile application for building and configuring a custom PC. Created as a graduation project using .NET MAUI and SQLite.

## 🚀 Features

- 🔍 Component selection: CPU, GPU, Motherboard, RAM, PSU, Storage, Case
- ⚙️ Compatibility checking between selected hardware
- 🌐 Multilingual interface: 🇬🇧 English, 🇷🇺 Russian, 🇷🇴 Romanian
- 🧾 Export selected build to PDF
- 🔐 Admin panel for managing components
- 💾 Local database with SQLite
- 🌙 Dark theme and simplified beginner mode

## 🎥 Demo Video

https://youtube.com/shorts/XlzO6PgaBgI?feature=share

Compatibility Logic
The application automatically checks for compatibility between selected PC components to help users build a functional configuration. Compatibility checks include:

CPU ↔ Motherboard
Ensures that the selected CPU socket matches the motherboard socket.

RAM ↔ Motherboard
Verifies RAM type (e.g., DDR4) is supported by the motherboard.

GPU ↔ PSU
Recommends a PSU with sufficient wattage based on the GPU's power requirements.

Motherboard ↔ Case
Validates that the case supports the motherboard form factor (e.g., ATX, Micro-ATX).

Storage ↔ Motherboard
Filters only compatible storage devices (e.g., M.2, SATA) based on motherboard slots.

PSU ↔ Case
Ensures the selected PSU fits into the chosen case size (ATX, SFX, etc).

RAM Slots Check
Prevents adding more RAM sticks than the motherboard supports.

The system dynamically filters and suggests only compatible options as the user builds the configuration. If a component becomes incompatible due to a new selection (e.g., switching to a different CPU), the app will update available options accordingly.


