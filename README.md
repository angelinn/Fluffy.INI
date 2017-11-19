# Fluffy.Ini
[![Build status](https://ci.appveyor.com/api/projects/status/9pq825fl7kgpnghg?svg=true)](https://ci.appveyor.com/project/betrakiss/fluffy-ini)

## What is Fluffy.Ini?
Fluffy.Ini is a .NET Standard and .NET Framework serializer that converts classes to ini configuration files.


## Sections
1. [How to use?](#how-to-use)

2. [Serialization](#serialization)

3. [Deserialization](#deserialization)

4. [FluffyIgnore](#fluffyignore)

5. [FluffyComment](#fluffycomment)

6. [Why?](#why)

7. [License](#license)


## How to use?
The main class is FluffyConverter.

### Two types of configuration are available 
* Single section - one class with primitive types
* Multiple sections - one root class with section classes as properties

*Can you say that again?*

**Sure.**

### Serialization

* Single section

Example:

```C#
public class Settings
{
    public string Resolution { get; set; }
    public int Volume { get; set; }
}

Settings settings = new Settings
{
       Resolution = "1920x1080",
       Volume = 80
};

string ini = FluffyConverter.SerializeObject(settings);
File.WriteAllText("config.ini", ini);
```

produces

```ini
[Settings]
Resolution=1920x1080
Volume=80

```

* Multiple sections - one class for the root object and one class per section with only primitive types.

Example:

```C#
public class DisplaySettings
{
    public string Resolution { get; set; }
}

public class VolumeSettings
{
    public int Volume { get; set; }
}

public class Settings
{
    public DisplaySettings Display { get; set; }
    public VolumeSettings Volume { get; set; }
}

Settings settings = new Settings
{
    Display = new DisplaySettings
    {
        Resolution = "1920x1080"
    },
    Volume = new VolumeSettings
    {
        Volume = 80
    }
};

string ini = FluffyConverter.SerializeObject(settings);
File.WriteAllText("config.ini", ini);
```
produces

```ini
[Display]
Resolution=1920x1080

[Volume]
Volume=80

```

### Deserialization
```C#
string ini = File.ReadAllText("config.ini");
Settings settings = FluffyConverter.DeserializeObject<Settings>(ini);
```

### FluffyIgnore
You can ignore a certain property putting the ```[FluffyIgnore]``` attribute above it
```C#
public class Settings
{
    [FluffyIgnore]
    public int SettingID { get; set; }
    public int Volume { get; set; }
}
```


### FluffyComment
Want a comment in your ini file? Sure.
```C#
public class Settings
{
    [FluffyComment("Wow. What a configuration setting!")]
    public int Volume { get; set; }
}
```

## Why?
I was working on a project at work that needed a configuration file. This project was using .NET Standard 1.4 and I could not find a suitable library that could serialize a class to an ini file. So, there we go!

## License
This project is licensed under the terms of the MIT license.
