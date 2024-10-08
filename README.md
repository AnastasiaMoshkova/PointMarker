## PointMarker - Приложение для разметки данных двигательной активности 

|Разметка сигналов двигательной активности рук|Разметка сигналов мимики|
|:-:|:-:|
|<img width="957" alt="20" src="https://github.com/user-attachments/assets/507eb313-760b-4005-a580-2fd02985993d">|<img width="958" alt="21" src="https://github.com/user-attachments/assets/aaeecb51-b480-4379-ad55-7c093033881b">|


### Для работы с приожением: 
 - При запуске в Relealse в PointMarker.exe.config заменить путь к устаноленному python и DLL (в config задано для python310)
 - Результаты сохраняются в папке Result в одной директории с .exe
 - Для работы .py скриптов нужно убедиться, что установлены следующие python модули в той версии питона, которая используется в приложении

```
import csv
import json
import math
import numpy as np
import statistics
from statsmodels.nonparametric.smoothers_lowess import lowess
from scipy import loadtxt, optimize
from scipy.signal import argrelmax, argrelmin
```

### Для сборки:
Установить NuGet пакеты:
 - Newtonsoft.Json v 12.0.2
 - pythonnet v 3.0.2
 - System.IO.FileSystem v 4.3.0
 - Microsoft.Xaml.Behaviors v 1.1.135

### Окружение, в котором тестировалось приложение:
- Windows 8
- Microsoft Visual Studio Community 2022 (64-разрядная версия) - Версия 17.7.3
  


