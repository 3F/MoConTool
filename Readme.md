# [MoConTool](https://github.com/3F/MoConTool)

A variety of patches and tweaks for your favorite mouse.

[![Build status](https://ci.appveyor.com/api/projects/status/t6icxagsil8kc6fy/branch/master?svg=true)](https://ci.appveyor.com/project/3Fs/mocontool/branch/master)
[![release-src](https://img.shields.io/github/release/3F/MoConTool.svg)](https://github.com/3F/MoConTool/releases/latest)
[![License](https://img.shields.io/badge/License-MIT-74A5C2.svg)](https://github.com/3F/MoConTool/blob/master/License.txt)

**Download:** [/releases](https://github.com/3F/MoConTool/releases) ( [latest](https://github.com/3F/MoConTool/releases/latest) )

## License

The [MIT License (MIT)](https://github.com/3F/MoConTool/blob/master/License.txt)

## Why MoConTool ?

![](https://github.com/3F/MoConTool/blob/master/Resources/MoConTool.png)

It provides different filters that can help/improve your favorite mouse. 
Especially when your device from high-end class with high cost (about 300$ and above) and noticed (in one beautiful day) some strange behaviour.

Before new buying, now you can try this because it's **absolutly free and open**. Moreover, architecture of MoConTool is flexible to add easily any other filter to mange your mouse. You can also try this by the sample below.

But in general, this tool helps not only with 'double-clicks' problem, it also should help to recover codes from connection problem of your wireless device, and much more.

![Interrupted click (Connection recovery)](https://github.com/3F/MoConTool/blob/master/Resources/ill_mouse_interrupted_click.gif)

By the way, the classical 'double-click' problem can be resolved is very easy without any software:

* the 3Pin micro-switch component is standard for 90% mouses even for high-end class ! The final sum of this should be $0.08 or less for each switch ! a trivial soldering with your favorite soldering iron and you mouse is ready for new races :) And anyway, for any problems with devices from low-end class - the more right and easy way, just to buy new similar device.

### Command-Line

Just click [cmd] button to generate automatically final arguments from selected options, for example:

```bash
MoConTool -InterruptedClick LMR 110;43;251 -MixedClicks L 1 -DoubleClicks L 118
```

### Configuration

The default values are very specific for specific device, so you need configure & test all this before using. 

But for any critical situation just press: `[Ctrl + Alt + RShift] + Z` to unplug this from system.

## Sample of filter

If you're software developer, you can also extend this by new filter. It easy by our flexible platform:

*(the plugin system can be implemented later if it's will be required by someone)*

```csharp
public class MyMouseFilter: FilterAbstract, IMouseListener
{
    public override FilterResult msg(int nCode, WPARAM wParam, LPARAM lParam)
    {
        if(SysMessages.Eq(wParam, SysMessages.WM_MBUTTONDOWN))
        {
            LSender.Send(this, $"Abort WM_MBUTTONDOWN", Message.Level.Info);
            trigger();
            return FilterResult.Abort;
        }

        return FilterResult.Continue;
    }

    public MouseFilter()
        : base("MyMouseFilter")
    {
        Handler = MouseState.Flags.Middle | MouseState.Flags.Down | MouseState.Flags.Up;
    }
}
...
IMouseListener fmm = listener(typeof(MyMouseFilter));
fic.Triggering += (object sender, DataArgs<ulong> e) => {
    uiAction(() => ...);
};
```

All available filters for sample - **[here](https://github.com/3F/MoConTool/tree/master/MoConTool/Filters)**

## &

[![Donate](https://www.paypalobjects.com/en_US/i/btn/btn_donate_SM.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=entry%2ereg%40gmail%2ecom&lc=US&item_name=3F%2dOpenSource%20%5b%20github%2ecom%2f3F&currency_code=USD&bn=PP%2dDonationsBF%3abtn_donate_SM%2egif%3aNonHosted)
