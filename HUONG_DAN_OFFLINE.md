# Phuong an dang nhap offline (ban thu nghiem ma nguon)

## Trang thai

Da tao class C# `OfflineAccountStore` **moi**, khong sao chep ma nguon ben thu ba. Chua tich hop vao `iAuto.exe`, chua build hoac kiem thu tren Windows. Khong co tai khoan/mat khau mac dinh trong GitHub.

## Phat hien tu repository tham khao

- Launcher C# .NET Framework 4.5 x86.
- `iAuto Launcher/m.cs` goi `dark.php?cmd=login` de xac thuc, sau do goi `dark.php?cmd=gui` de tai bo GUI ve va nap assembly tu ZIP.
- `iAuto Launcher/bin/Release/iAuto.zip` chua **mot tep iAuto_DarkMode.exe**, khong chung minh duoc co DLL GUI tuong thich voi `GUI.Create`.
- Do do **khong the khang dinh** chi sua login la du de chay offline. Can GUI hop le va tai san day du duoc phep su dung.
- Tep `-iAuto.ini` trong nguon co truong mat khau plaintext. Khong dua vao repo nay.

## Cach tich hop sau khi xac minh quyen

1. Tao ban sao ma nguon duoc phep su dung va mot nhanh `original`; tuyet doi khong sua `iAuto.exe` goc.
2. Them `OfflineAccountStore.cs` vao project WinForms, tich hop `Create` vao dang ky va `Verify` vao dang nhap.
3. Tao tai khoan dau tien tai may Windows qua `OfflineAccountStore.Create`. Mat khau khong duoc dat trong GitHub.
4. Khong gui mat khau offline vao GUI DLL bat ky; khong bo qua cap phep hoac xac thuc chuc nang ben trong DLL.
5. Neu GUI phu thuoc may chu, can cung cap module GUI cục bo duoc cap quyen hoặc xay dung module thay the cua rieng ban, khong chi gia lap ket qua login.
6. Build tu **ma nguon da sua** tren Windows, .NET Framework 4.5, x86; thu nghiem trong VM va ngat mang.
7. Kiem tra `git diff`, test dung/sai mat khau, test tai lan dau, va test chuc nang.

## Bao mat va ban quyen

- Repository hien cong khai. Chu so huu can doi sang Private trong GitHub Settings.
- Khong sao chep ma nguon ben thu ba khi chua co phep su dung/chinh sua/phan phoi.
- Khong dua `account.dat`, token, mat khau hay file EXE/DLL khong kiem chung len GitHub.
- `account.dat` duoc luu tai `%LOCALAPPDATA%\\iAutoOffline\\account.dat` voi salt ngau nhien va PBKDF2.
