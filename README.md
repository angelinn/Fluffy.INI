# Fluffy.Ini
[![Build status](https://ci.appveyor.com/api/projects/status/9pq825fl7kgpnghg?svg=true)](https://ci.appveyor.com/project/betrakiss/fluffy-ini)

## What is Fluffy.Ini?
Fluffy.Ini is a .NET Standard and .NET Framework serializer that converts classes to ini configuration files.

## How to use?
The main class is FluffyConverter.


* How do I make my classes good for serialization?

The way the serializer works is - you need one class for the root object and one class per section with only primitive types.

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
```
* Serializing example
```C#
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

The code produces the following config.ini:
```ini
[Display]
Resolution=1920x1080

[Volume]
Volume=80

```

## Why?
I was working on a project at work that needed a configuration file. This project was using .NET Standard 1.4 and I could not find a suitable library that could serialize a class to an ini file. So, there we go!
