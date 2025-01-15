---
Title: The ChartPlot Studio Plugin
Subtitle: Create and Visualize Dynamic Charts with ChartPlot Studio in ScriptRunner
Category: Cookbook
Author: Peter van de Pas
keywords: [CookBook, ChartPlot Studio, Visualization, Chart]
table-use-row-colors: true
table-row-color: "D3D3D3"
toc: true
toc-title: Table of Content
toc-own-page: true
---

# Recipe: Create and Visualize Dynamic Charts with ChartPlot Studio in ScriptRunner

## Goal

Learn how to use the **ChartPlot Studio Plugin** in ScriptRunner to create and interact with dynamic charts.
This recipe covers configuring different chart types, setting data, and displaying the charts interactively.

## Overview

This recipe demonstrates how to:

1. Configure chart types (Pie, Line, Histogram, Bar).
2. Customize chart properties such as labels, colors, and titles.
3. Interactively display and manage charts using the dialog service.
4. Switch between multiple charts in a single script execution.

By the end of this tutorial, you'll have a script that creates, displays,
and manages dynamic charts using ChartPlot Studio.

---

## Steps

### 1. Define Task Metadata

Add metadata to the script for identification and categorization:

```csharp
/*
{
    "TaskCategory": "Plugins",
    "TaskName": "ChartPlot Studio Demo Script",
    "TaskDetail": "A test script for the ChartPlot Studio Plugin",
    "RequiredPlugins": ["ChartPlot Studio"]
}
*/
```

### 2. Initialize the ChartPlot Studio Plugin

Set up the plugin and logger, then initialize the chart plotter:

```csharp
var logger = GetLogger("ChartPlotter");
var chartPlotter = new ChartPlotter(logger: logger);
```

### 3. Configure and Display a Pie Chart

Set up a Pie chart with specific configuration and display it using the dialog service:

```csharp
chartPlotter.Setup(
    chartType: ChartType.Pie, 
    title: "Test Chart", 
    labels: new[] { "Category A", "Category B", "Category C" },
    colors: new[] { "Red", "Blue", "Green" },
    showLegend: true,
    pieExplodeFraction: 0.1,
    pieSliceLabelDistance: 1.2
);
chartPlotter.SetData(new[] { 45.0, 30.0, 25.0 });

var chartPlotStudio = new ChartPlotStudio();
var result = await chartPlotStudio.GetChartPlotDialogAsync(
    chartPlotter,
    "Chart Dialog Test",
    800, // Width
    600  // Height
);

Dump($"First Chart Result: {result}");
```

### 4. Configure and Display a Line Chart

If the first dialog closes successfully, configure and display a Line chart:

```csharp
if (result != null)
{
    chartPlotter.Setup(
        chartType: ChartType.Line,
        title: "Test Line Chart",
        labels: null,
        colors: null,
        keepColorsNull: true,
        showLegend: false
    );
    chartPlotter.SetData(new[] { 10.0, 20.0, 15.0, 25.0, 30.0, 20.0 });

    var secondResult = await chartPlotStudio.GetChartPlotDialogAsync(
        chartPlotter,
        "Line Chart Dialog",
        800, // Width
        600  // Height
    );

    Dump($"Second Chart Result: {secondResult}");
}
```

### 5. Configure and Display a Histogram

If the second dialog closes successfully, configure and display a Histogram:

```csharp
if (secondResult != null)
{
    chartPlotter.Setup(
        chartType: ChartType.Histogram,
        title: "Test Histogram Chart",
        labels: null,
        colors: new[] { "Purple" },
        showLegend: true,
        yLabel: "Frequency",
        xLabel: "Values",
        histogramBinCount: 5
    );
    chartPlotter.SetData(new[] { 10.0, 20.0, 20.0, 15.0, 25.0, 30.0, 20.0, 25.0, 15.0 });

    var thirdResult = await chartPlotStudio.GetChartPlotDialogAsync(
        chartPlotter,
        "Histogram Chart Dialog",
        800, // Width
        600  // Height
    );

    Dump($"Third Chart Result: {thirdResult}");
}
```

---

## Example Script

Here’s the complete script for reference:

```csharp
/*
{
    "TaskCategory": "Plugins",
    "TaskName": "ChartPlot Studio Demo Script",
    "TaskDetail": "A test script for the ChartPlot Studio Plugin",
    "RequiredPlugins": ["ChartPlot Studio"]
}
*/

var chartPlotStudio = new ChartPlotStudio();
var logger = GetLogger("ChartPlotter");
var chartPlotter = new ChartPlotter(logger: logger);

// Configure and display Pie chart
chartPlotter.Setup(
    chartType: ChartType.Pie, 
    title: "Test Chart", 
    labels: new[] { "Category A", "Category B", "Category C" },
    colors: new[] { "Red", "Blue", "Green" },
    showLegend: true,
    pieExplodeFraction: 0.1,
    pieSliceLabelDistance: 1.2
);
chartPlotter.SetData(new[] { 45.0, 30.0, 25.0 });

var result = await chartPlotStudio.GetChartPlotDialogAsync(
    chartPlotter,
    "Chart Dialog Test",
    800, // Width
    600  // Height
);

Dump($"First Chart Result: {result}");

if (result != null)
{
    // Configure and display Line chart
    chartPlotter.Setup(
        chartType: ChartType.Line,
        title: "Test Line Chart",
        labels: null,
        colors: null,
        keepColorsNull: true,
        showLegend: false
    );
    chartPlotter.SetData(new[] { 10.0, 20.0, 15.0, 25.0, 30.0, 20.0 });

    var secondResult = await chartPlotStudio.GetChartPlotDialogAsync(
        chartPlotter,
        "Line Chart Dialog",
        800, // Width
        600  // Height
    );

    Dump($"Second Chart Result: {secondResult}");

    if (secondResult != null)
    {
        // Configure and display Histogram
        chartPlotter.Setup(
            chartType: ChartType.Histogram,
            title: "Test Histogram Chart",
            labels: null,
            colors: new[] { "Purple" },
            showLegend: true,
            yLabel: "Frequency",
            xLabel: "Values",
            histogramBinCount: 5
        );
        chartPlotter.SetData(new[] { 10.0, 20.0, 20.0, 15.0, 25.0, 30.0, 20.0, 25.0, 15.0 });

        var thirdResult = await chartPlotStudio.GetChartPlotDialogAsync(
            chartPlotter,
            "Histogram Chart Dialog",
            800, // Width
            600  // Height
        );

        Dump($"Third Chart Result: {thirdResult}");
    }
}

return "ChartPlot Studio Demo completed";
```

---

## Expected Output

1. **Pie Chart**: Displays a dialog with a Pie chart and labeled slices.
2. **Line Chart**: Displays a dialog with a Line chart showing trends over data points.
3. **Histogram**: Displays a dialog with a Histogram chart showing data distribution.

---

## Tips & Notes

- **Dynamic Chart Configuration**: Modify chart properties such as colors, labels, and titles dynamically before
  displaying.
- **Interactivity**: Utilize dialogs to make the charts interactive for users.
- **Multiple Chart Types**: Experiment with different chart types (Pie, Line, Histogram, Bar) for diverse
  visualizations.
- **Customization**: Use optional parameters to customize aspects like legend visibility, pie slice explosion, and axis
  labels.
