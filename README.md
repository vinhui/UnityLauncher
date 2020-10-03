# UnityLauncher

This project is a simple CLI wrapper around the Unity Hub. 

This project was mainly developed on and for Linux, but should also work other operating systems.

## Example Usage

### Open a project:
`$ UnityLauncher launch --hub-path ~/UnityHub.AppImage --project ~/projects/MyUnityProject`

### Install a Unity version

Install using a hub uri that you can get from the download archive:
`$ UnityLauncher install --hub-path ~/UnityHub.AppImage --huburi unityhub://2020.1.6f1/fc477ca6df10`

Install a new Unity release:
`$ UnityLauncher install --hub-path ~/UnityHub.AppImage --version 2018.4.27f1`
This won't work with all versions. This will usually only work for the latest minor releases, for example `2019.4.latest` or `2020.1.latest`. For others you might also need to add a `--changeset` flag:
`$ UnityLauncher install --hub-path ~/UnityHub.AppImage --version 2020.1.0f1 --changeset 2ab9c4179772 `

There's also a `--modules` options which allows you to install additional modules. The following is a full list of available modules to choose from:

- `documentation`: Offline Documentation
- `standardassets`: Standard Assets
- `example`: Example Project
- `monodevelop`: MonoDevelop / Unity Debugger
- `vuforia-ar`: Vuforia Augmented Reality Support
- Platforms:
  - `android`: Android Build Support
    - `android-sdk-ndk-tools`: Android SDK & NDK Tools
    - `android-open-jdk`: OpenJDK
    - `android-sdk-platform-tools`: Android SDK Platform Tools
    - `android-sdk-build-tools`: Android SDK Build Tools
    - `android-sdk-platforms`: Android SDK Platforms
    - `android-ndk`: Android NDK
    - `android-open-jdk`: Android Open JDK
  - `ios`: iOS Build Support
  - `windows`: Windows Build Support
  - `windows-mono`: Windows Build Support (Mono)
  - `linux-il2cpp`: Linux Build Support (IL2CPP)
  - `linux-mono`: Linux Build Support (Mono)
  - `mac-il2cpp`: Mac Build Support (IL2CPP)
  - `mac-mono`: Mac Build Support (Mono)
  - `webgl`: WebGL Build Support
  - `lumin`: Lumin OS (Magic Leap) Build Support
  - `appletv`: tvOS Build Support
  - `samsung`: SamsungTV Build Support
  - `tizen`: Tizen Build Support
  - `facebook-games`: Facebook Gameroom Build Support
- Language Packs 
  - `language-ja`
  - `language-ko`
  - `language-zh-cn`
  - `language-zh-hant`
  - `language-zh-hans`

This list is a combination of all items in the `modules.json` from a installation and `UnityHub.AppImage --headless help`. There might be more modules.

### Help

`$ UnityLauncher help` or `$ UnityHelper help install` to get more info for a specific command.

## Terminal Shortcut

Add the following snippet to your `~/.zshrc` or `~/.bashrc` file to have shortcut to launch a project.

```bash
function unity() {
    local oldDir="$(pwd)"
    cd "$(find . -maxdepth 1 -type d -name '*Unity*' -print -quit)"
    ~/UnityLauncher launch --hub-path ~/UnityHub.AppImage --project "$(pwd)" --install-if-needed
    cd "$oldDir"
}
```

After that run `$ source ~/.zshrc` or `$ source ~/.bashrc`. Now you can run `$ unity` in the terminal in your project directory and it will launch the correct Unity version (and install if it's not installed yet). The function will also search for any `*Unity*` child directories. This allows the following to also work when using the `$ unity` shortcut.

```
.
└── MyProjectRoot			<- current dir
    ├── SomeOtherDir
    └── UnityProject
        ├── Assets
        ├── Packages
        └── ProjectSettings
```