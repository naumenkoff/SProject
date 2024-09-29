using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SProject.WPF.Abstractions;

[SuppressMessage("ReSharper", "UnusedTypeParameter")]
public interface IViewOf<TVm> where TVm : ObservableObject;