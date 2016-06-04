param($installPath, $toolsPath, $package, $project)

$Leap_Item = $project.ProjectItems.Item("Leap.dll")
$Leap_OutputProp = $Leap_Item.Properties.Item("CopyToOutputDirectory")
$Leap_OutputProp.Value = 2

$LeapCSharp_Item = $project.ProjectItems.Item("LeapCSharp.dll")
$LeapCSharp_OutputProp = $LeapCSharp_Item.Properties.Item("CopyToOutputDirectory")
$LeapCSharp_OutputProp.Value = 2
