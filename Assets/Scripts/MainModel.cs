using System.Collections.Generic;
using UnityEngine;

public class MainModel
{
    [Inject]
    public IStorageService StorageService { get; private set; }
    [Inject]
    public LocalizationService LocalizationService { get; private set; }
}