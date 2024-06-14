using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SProject.WPF.Abstractions;

public interface IViewOf<[SuppressMessage("ReSharper", "UnusedTypeParameter")] TVm> where TVm : ObservableObject;