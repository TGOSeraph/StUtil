using System;
using System.Runtime.InteropServices;

namespace StUtil.Native.Internal
{
    public static partial class NativeMethods
    {
        [DllImport("ntdll.dll")]
        public static extern int NtQueryInformationProcess(IntPtr processHandle, int processInformationClass, ref NativeStructs.PROCESS_BASIC_INFORMATION processInformation, int processInformationLength, out int returnLength);
        
        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        /// <summary>
        /// <para>Opens an existing local process object.</para>
        /// </summary>
        /// <param name="dwDesiredAccess">If the caller has enabled the SeDebugPrivilege privilege, the requested access is granted regardless of the contents of the security descriptor.</param>
        /// <param name="bInheritHandle">If this value is TRUE, processes created by this process will inherit the handle. Otherwise, the processes do not inherit this handle.</param>
        /// <param name="dwProcessId">If the specified process is the System Process (0x00000000), the function fails and the last error code is ERROR_INVALID_PARAMETER. If the specified process is the Idle process or one of the CSRSS processes, this function fails and the last error code is ERROR_ACCESS_DENIED because their access restrictions prevent user-level code from opening them.</param>
        /// <returns>
        /// <para>If the function succeeds, the return value is an open handle to the specified process.</para>
        /// <para>If the function fails, the return value is NULL. To get extended error information, call
        /// etLastError.</para>
        /// </returns>
        /// <remarks>
        /// <para>To open a handle to another local process and obtain full access rights, you must enable the SeDebugPrivilege privilege. For more information, see Changing Privileges in a Token.</para>
        /// <para>The handle returned by the
        /// penProcess function can be used in any function that requires a handle to a process, such as the
        /// ait functions, provided the appropriate access rights were requested.</para>
        /// <para>When you are finished with the handle, be sure to close it using the CloseHandle function.</para>
        /// </remarks>
        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, uint dwProcessId);

        /// <summary>
        /// Retrieves the address of an exported function or variable from the specified dynamic-link library (DLL).
        /// </summary>
        /// <param name="hModule">The GetProcAddress function does not retrieve addresses from modules that were loaded using the LOAD_LIBRARY_AS_DATAFILE flag. For more information, see LoadLibraryEx.</param>
        /// <param name="lpProcName">The function or variable name, or the function's ordinal value. If this parameter is an ordinal value, it must be in the low-order word; the high-order word must be zero.</param>
        /// <returns>
        /// <para>If the function succeeds, the return value is the address of the exported function or variable.</para>
        /// <para>If the function fails, the return value is NULL. To get extended error information, call
        /// GetLastError.</para>
        /// </returns>
        /// <remarks>
        /// <para>The spelling and case of a function name pointed to by lpProcName must be identical to that in the EXPORTS statement of the source DLL's module-definition (.def) file. The exported names of functions may differ from the names you use when calling these functions in your code. This difference is hidden by macros used in the SDK header files. For more information, see
        /// conventions for Function Prototypes.</para>
        /// <para>The lpProcName parameter can identify the DLL function by specifying an ordinal value associated with the function in the EXPORTS statement.
        /// GetProcAddress verifies that the specified ordinal is in the range 1 through the highest ordinal value exported in the .def file. The function then uses the ordinal as an index to read the function's address from a function table.</para>
        /// <para>If the .def file does not number the functions consecutively from 1 to N (where N is the number of exported functions), an error can occur where
        /// GetProcAddress returns an invalid, non-NULL address, even though there is no function with the specified ordinal.</para>
        /// </remarks>
        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        /// <summary>
        /// <para>Retrieves a module handle for the specified module. The module must have been loaded by the calling process.</para>
        /// </summary>
        /// <param name="lpModuleName">If this parameter is NULL,
        /// etModuleHandle returns a handle to the file used to create the calling process (.exe file).</param>
        /// <returns>
        /// <para>If the function succeeds, the return value is a handle to the specified module.</para>
        /// <para>If the function fails, the return value is NULL. To get extended error information, call
        /// etLastError.</para>
        /// </returns>
        /// <remarks>
        /// <para>The returned handle is not global or inheritable. It cannot be duplicated or used by another process.</para>
        /// <para>If lpModuleName does not include a path and there is more than one loaded module with the same base name and extension, you cannot predict which module handle will be returned. To work around this problem, you could specify a path, use side-by-side assemblies, or use GetModuleHandleEx to specify a memory location rather than a DLL name.</para>
        /// <para>The
        /// etModuleHandle function returns a handle to a mapped module without incrementing its reference count. However, if this handle is passed to the FreeLibrary function, the reference count of the mapped module will be decremented. Therefore, do not pass a handle returned by GetModuleHandle to the
        /// reeLibrary function. Doing so can cause a DLL module to be unmapped prematurely.</para>
        /// <para>This function must be used carefully in a multithreaded application. There is no guarantee that the module handle remains valid between the time this function returns the handle and the time it is used. For example, suppose that a thread retrieves a module handle, but before it uses the handle, a second thread frees the module. If the system loads another module, it could reuse the module handle that was recently freed. Therefore, the first thread would have a handle to a different module than the one intended.</para>
        /// </remarks>
        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        /// <summary>
        /// <para>Reserves or commits a region of memory within the virtual address space of a specified process. The
        /// function initializes the memory it allocates to zero, unless MEM_RESET is
        /// used.</para>
        /// </summary>
        /// <param name="hProcess">The handle must have the PROCESS_VM_OPERATION access right. For more information,
        /// see
        /// Process Security and Access Rights.</param>
        /// <param name="lpAddress">If you are reserving memory, the function rounds this address down to the nearest multiple of the allocation
        /// granularity.</param>
        /// <param name="dwSize">If lpAddress is NULL, the function rounds
        /// dwSize up to the next page boundary.</param>
        /// <param name="flAllocationType">
        /// alueMeaningMEM_COMMIT
        /// x00001000
        /// llocates memory charges (from the overall size of memory and the paging files on disk) for the specified
        /// reserved memory pages. The function also guarantees that when the caller later initially accesses the memory,
        /// the contents will be zero. Actual physical pages are not allocated unless/until the virtual addresses are
        /// actually accessed.
        /// o reserve and commit pages in one step, call
        /// VirtualAllocEx with
        /// MEM_COMMIT | MEM_RESERVE.
        /// ttempting to commit a specific address range by specifying MEM_COMMIT without
        /// MEM_RESERVE and a non-NULL lpAddress fails unless the entire range has already been reserved. The resulting
        /// error code is ERROR_INVALID_ADDRESS.
        /// n attempt to commit a page that is already committed does not cause the function to fail. This means that
        /// you can commit pages without first determining the current commitment state of each page.
        /// EM_RESERVE
        /// x00002000
        /// eserves a range of the process's virtual address space without allocating any actual physical storage in
        /// memory or in the paging file on disk.
        /// ou commit reserved pages by calling
        /// VirtualAllocEx again with
        /// MEM_COMMIT. To reserve and commit pages in one step, call
        /// VirtualAllocEx with
        /// MEM_COMMIT | MEM_RESERVE.
        /// ther memory allocation functions, such as malloc and
        /// LocalAlloc, cannot use reserved memory until it has
        /// been released.
        /// EM_RESET
        /// x00080000
        /// ndicates that data in the memory range specified by lpAddress and
        /// dwSize is no longer of interest. The pages should not be read from or written to
        /// the paging file. However, the memory block will be used again later, so it should not be decommitted. This
        /// value cannot be used with any other value.
        /// sing this value does not guarantee that the range operated on with MEM_RESET
        /// will contain zeros. If you want the range to contain zeros, decommit the memory and then recommit it.
        /// hen you use MEM_RESET, the
        /// VirtualAllocEx function ignores the value of
        /// fProtect. However, you must still set fProtect to a valid
        /// protection value, such as PAGE_NOACCESS.
        /// irtualAllocEx returns an error if you use
        /// MEM_RESET and the range of memory is mapped to a file. A shared view is only
        /// acceptable if it is mapped to a paging file.
        /// EM_RESET_UNDO
        /// x1000000
        /// EM_RESET_UNDO should only be called on an address range to which
        /// MEM_RESET was successfully applied earlier. It indicates that the data in the
        /// specified memory range specified by lpAddress and dwSize
        /// is of interest to the caller and attempts to reverse the effects of MEM_RESET. If
        /// the function succeeds, that means all data in the specified address range is intact. If the function fails,
        /// at least some of the data in the address range has been replaced with zeroes.
        /// his value cannot be used with any other value. If MEM_RESET_UNDO is called on an
        /// address range which was not MEM_RESET earlier, the behavior is undefined. When you
        /// specify MEM_RESET, the
        /// VirtualAllocEx function ignores the value of
        /// flProtect. However, you must still set flProtect to a
        /// valid protection value, such as PAGE_NOACCESS.
        /// indows Server 2008 R2, Windows 7, Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP:  The MEM_RESET_UNDO flag is not supported until Windows 8 and
        /// Windows Server 2012.</param>
        /// <param name="flProtect">The memory protection for the region of pages to be allocated. If the pages are being committed, you can
        /// specify any one of the
        /// memory protection constants.</param>
        /// <returns>
        /// <para>If the function succeeds, the return value is the base address of the allocated region of pages.</para>
        /// <para>If the function fails, the return value is NULL. To get extended error information,
        /// call GetLastError.</para>
        /// </returns>
        /// <remarks>
        /// <para>Each page has an associated page state. The
        /// VirtualAllocEx function can perform the following
        /// operations:</para>
        /// ommit a region of reserved pages
        /// eserve a region of free pages
        /// imultaneously reserve and commit a region of free pages
        /// <para>VirtualAllocEx cannot reserve a reserved page. It
        /// can commit a page that is already committed. This means you can commit a range of pages, regardless of whether
        /// they have already been committed, and the function will not fail.</para>
        /// <para>You can use VirtualAllocEx to reserve a block of
        /// pages and then make additional calls to VirtualAllocEx
        /// to commit individual pages from the reserved block. This enables a process to reserve a range of its virtual
        /// address space without consuming physical storage until it is needed.</para>
        /// <para>If the lpAddress parameter is not NULL, the function uses
        /// the lpAddress and dwSize parameters to compute the region of
        /// pages to be allocated. The current state of the entire range of pages must be compatible with the type of
        /// allocation specified by the flAllocationType parameter. Otherwise, the function fails
        /// and none of the pages is allocated. This compatibility requirement does not preclude committing an already
        /// committed page; see the preceding list.</para>
        /// <para>To execute dynamically generated code, use
        /// VirtualAllocEx to allocate memory and the
        /// VirtualProtectEx function to grant
        /// PAGE_EXECUTE access.</para>
        /// <para>The VirtualAllocEx function can be used to reserve
        /// an Address Windowing Extensions
        /// (AWE) region of memory within the virtual address space of a specified process. This region of memory can then be
        /// used to map physical pages into and out of virtual memory as required by the application. The
        /// MEM_PHYSICAL and MEM_RESERVE values must be set in the
        /// AllocationType parameter. The MEM_COMMIT value must not be
        /// set. The page protection must be set to PAGE_READWRITE.</para>
        /// <para>The VirtualFreeEx function can decommit a committed
        /// page, releasing the page's storage, or it can simultaneously decommit and release a committed page. It can also
        /// release a reserved page, making it a free page.</para>
        /// <para>When creating a region that will be executable, the calling program bears responsibility for ensuring cache
        /// coherency via an appropriate call to
        /// FlushInstructionCache once the code has been set
        /// in place. Otherwise attempts to execute code out of the newly executable region may produce unpredictable
        /// results.</para>
        /// </remarks>
        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize, uint flAllocationType, uint flProtect);

        /// <summary>
        /// <para>Releases, decommits, or releases and decommits a region of memory within the virtual address space of a specified process.</para>
        /// </summary>
        /// <param name="hProcess">The handle must have the PROCESS_VM_OPERATION access right. For more information, see
        /// rocess Security and Access Rights.</param>
        /// <param name="lpAddress">If the dwFreeType parameter is MEM_RELEASE, lpAddress must be the base address returned by the
        /// irtualAllocEx function when the region is reserved.</param>
        /// <param name="dwSize">If the dwFreeType parameter is MEM_RELEASE, dwSize must be 0 (zero). The function frees the entire region that is reserved in the initial allocation call to
        /// irtualAllocEx.</param>
        /// <param name="dwFreeType">
        /// alueMeaningMEM_DECOMMIT
        /// x4000
        /// ecommits the specified region of committed pages. After the operation, the pages are in the reserved state. The function does not fail if you attempt to decommit an uncommitted page. This means that you can decommit a range of pages without first determining their current commitment state.
        /// o not use this value with MEM_RELEASE.
        /// EM_RELEASE
        /// x8000
        /// eleases the specified region of pages. After the operation, the pages are in the free state. If you specify this value, dwSize must be 0 (zero), and lpAddress must point to the base address returned by the
        /// irtualAllocEx function when the region is reserved. The function fails if either of these conditions is not met.
        /// f any pages in the region are committed currently, the function first decommits, and then releases them.
        /// he function does not fail if you attempt to release pages that are in different states, some reserved and some committed. This means that you can release a range of pages without first determining the current commitment state.
        /// o not use this value with MEM_DECOMMIT.</param>
        /// <returns>
        /// <para>If the function succeeds, the return value is a nonzero value.</para>
        /// <para>If the function fails, the return value is 0 (zero). To get extended error information, call
        /// etLastError.</para>
        /// </returns>
        /// <remarks>
        /// <para>Each page of memory in a process virtual address space has a
        /// age State. The
        /// irtualFreeEx function can decommit a range of pages that are in different states, some committed and some uncommitted. This means that you can decommit a range of pages without first determining the current commitment state of each page. Decommitting a page releases its physical storage, either in memory or in the paging file on disk.</para>
        /// <para>If a page is decommitted but not released, its state changes to reserved. Subsequently, you can call
        /// irtualAllocEx to commit it, or
        /// irtualFreeEx to release it. Attempting to read from or write to a reserved page results in an access violation exception.</para>
        /// <para>The
        /// irtualFreeEx function can release a range of pages that are in different states, some reserved and some committed. This means that you can release a range of pages without first determining the current commitment state of each page. The entire range of pages originally reserved by
        /// irtualAllocEx must be released at the same time.</para>
        /// <para>If a page is released, its state changes to free, and it is available for subsequent allocation operations. After memory is released or decommitted, you can never refer to the memory again. Any information that may have been in that memory is gone forever. Attempts to read from or write to a free page results in an access violation exception. If you need to keep information, do not decommit or free memory that contains the information.</para>
        /// <para>The
        /// irtualFreeEx function can be used on an AWE region of memory and it invalidates any physical page mappings in the region when freeing the address space. However, the physical pages are not deleted, and the application can use them. The application must explicitly call
        /// reeUserPhysicalPages to free the physical pages. When the process is terminated, all resources are automatically cleaned up.</para>
        /// </remarks>
        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern bool VirtualFreeEx(IntPtr hProcess, [MarshalAs(UnmanagedType.AsAny)] object lpAddress, IntPtr dwSize, uint dwFreeType);

        /// <summary>
        /// <para>Writes data to an area of memory in a specified process. The entire area to be written to must be accessible or the operation fails.</para>
        /// </summary>
        /// <param name="hProcess">A handle to the process memory to be modified. The handle must have PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process.</param>
        /// <param name="lpBaseAddress">A pointer to the base address in the specified process to which data is written. Before data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for write access, and if it is not accessible, the function fails.</param>
        /// <param name="lpBuffer">A pointer to the buffer that contains data to be written in the address space of the specified process.</param>
        /// <param name="nSize">The number of bytes to be written to the specified process.</param>
        /// <param name="lpNumberOfBytesWritten">A pointer to a variable that receives the number of bytes transferred into the specified process. This parameter is optional. If lpNumberOfBytesWritten is NULL, the parameter is ignored.</param>
        /// <returns>
        /// <para>If the function succeeds, the return value is nonzero.</para>
        /// <para>If the function fails, the return value is 0 (zero). To get extended error information, call
        /// etLastError. The function fails if the requested write operation crosses into an area of the process that is inaccessible.</para>
        /// </returns>
        /// <remarks>
        /// <para>WriteProcessMemory copies the data from the specified buffer in the current process to the address range of the specified process. Any process that has a handle with PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process to be written to can call the function. Typically but not always, the process with address space that is being written to is being debugged.</para>
        /// <para>The entire area to be written to must be accessible, and if it is not accessible, the function fails.</para>
        /// </remarks>
        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, IntPtr nSize, out IntPtr lpNumberOfBytesWritten);

        /// <summary>
        /// <para>Creates a thread that runs in the virtual address space of another process.</para>
        /// </summary>
        /// <param name="hProcess">A handle to the process in which the thread is to be created. The handle must have the PROCESS_CREATE_THREAD, PROCESS_QUERY_INFORMATION, PROCESS_VM_OPERATION, PROCESS_VM_WRITE, and PROCESS_VM_READ access rights, and may fail without these rights on certain platforms. For more information, see
        /// rocess Security and Access Rights.</param>
        /// <param name="lpThreadAttributes">Windows XP:  The ACLs in the default security descriptor for a thread come from the primary or impersonation token of the creator. This behavior changed with Windows XP with SP2 and Windows Server 2003.</param>
        /// <param name="dwStackSize">The initial size of the stack, in bytes. The system rounds this value to the nearest page. If this parameter is 0 (zero), the new thread uses the default size for the executable. For more information, see
        /// hread Stack Size.</param>
        /// <param name="lpStartAddress">A pointer to the application-defined function of type LPTHREAD_START_ROUTINE to be executed by the thread and represents the starting address of the thread in the remote process. The function must exist in the remote process. For more information, see
        /// hreadProc.</param>
        /// <param name="lpParameter">A pointer to a variable to be passed to the thread function.</param>
        /// <param name="dwCreationFlags">
        /// alueMeaning
        ///
        /// he thread runs immediately after creation.
        /// REATE_SUSPENDED
        /// x00000004
        /// he thread is created in a suspended state, and does not run until the
        /// esumeThread function is called.
        /// TACK_SIZE_PARAM_IS_A_RESERVATION
        /// x00010000
        /// he dwStackSize parameter specifies the initial reserve size of the stack. If this flag is not specified, dwStackSize specifies the commit size.</param>
        /// <param name="lpThreadId">If this parameter is NULL, the thread identifier is not returned.</param>
        /// <returns>
        /// <para>If the function succeeds, the return value is a handle to the new thread.</para>
        /// <para>If the function fails, the return value is NULL. To get extended error information, call
        /// etLastError.</para>
        /// <para>Note that
        /// reateRemoteThread may succeed even if lpStartAddress points to data, code, or is not accessible. If the start address is invalid when the thread runs, an exception occurs, and the thread terminates. Thread termination due to a invalid start address is handled as an error exit for the thread's process. This behavior is similar to the asynchronous nature of
        /// reateProcess, where the process is created even if it refers to invalid or missing dynamic-link libraries (DLL).</para>
        /// </returns>
        /// <remarks>
        /// <para>The
        /// reateRemoteThread function causes a new thread of execution to begin in the address space of the specified process. The thread has access to all objects that the process opens.</para>
        /// <para>Terminal Services isolates each terminal session by design. Therefore,
        /// reateRemoteThread fails if the target process is in a different session than the calling process.</para>
        /// <para>The new thread handle is created with full access to the new thread. If a security descriptor is not provided, the handle may be used in any function that requires a thread object handle. When a security descriptor is provided, an access check is performed on all subsequent uses of the handle before access is granted. If the access check denies access, the requesting process cannot use the handle to gain access to the thread.</para>
        /// <para>If the thread is created in a runnable state (that is, if the CREATE_SUSPENDED flag is not used), the thread can start running before CreateThread returns and, in particular, before the caller receives the handle and identifier of the created thread.</para>
        /// <para>The thread is created with a thread priority of THREAD_PRIORITY_NORMAL. Use the
        /// etThreadPriority and
        /// etThreadPriority functions to get and set the priority value of a thread.</para>
        /// <para>When a thread terminates, the thread object attains a signaled state, which satisfies the threads that are waiting for the object.</para>
        /// <para>The thread object remains in the system until the thread has terminated and all handles to it are closed through a call to
        /// loseHandle.</para>
        /// <para>The
        /// xitProcess,
        /// xitThread,
        /// reateThread,
        /// reateRemoteThread functions, and a process that is starting (as the result of a
        /// reateProcess call) are serialized between each other within a process. Only one of these events occurs in an address space at a time. This means the following restrictions hold:</para>
        /// uring process startup and DLL initialization routines, new threads can be created, but they do not begin execution until DLL initialization is done for the process.
        /// nly one thread in a process can be in a DLL initialization or detach routine at a time.ExitProcess returns after all threads have completed their DLL initialization or detach routines.
        /// <para>A common use of this function is to inject a thread into a process that is being debugged to issue a break. However, this use is not recommended, because the extra thread is confusing to the person debugging the application and there are several side effects to using this technique:</para>
        /// t converts single-threaded applications into multithreaded applications.
        /// t changes the timing and memory layout of the process.
        /// t results in a call to the entry point of each DLL in the process.
        /// <para>Another common use of this function is to inject a thread into a process to query heap or other process information. This can cause the same side effects mentioned in the previous paragraph. Also, the application can deadlock if the thread attempts to obtain ownership of locks that another thread is using.</para>
        /// </remarks>
        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, IntPtr dwStackSize, IntPtr lpStartAddress, [MarshalAs(UnmanagedType.AsAny)] object lpParameter, uint dwCreationFlags, out IntPtr lpThreadId);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            [Out] byte[] lpBuffer,
            int dwSize,
            out IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            [Out, MarshalAs(UnmanagedType.AsAny)] object lpBuffer,
            int dwSize,
            out IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            IntPtr lpBuffer,
            int dwSize,
            out IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWow64Process([In] IntPtr processHandle,
             [Out, MarshalAs(UnmanagedType.Bool)] out bool wow64Process);

        /// <summary>
        /// Loads the specified module into the address space of the calling process. The specified
        /// module may cause other modules to be loaded.
        /// </summary>
        /// <param name="lpFileName">If the string specifies a full path, the function searches only that path for the module.</param>
        /// <returns>
        /// <para>If the function succeeds, the return value is a handle to the module.</para>
        /// <para>If the function fails, the return value is NULL. To get extended error information, call
        /// GetLastError.</para>
        /// </returns>
        /// <remarks>
        /// <para>To enable or disable error messages displayed by the loader during DLL loads, use the
        /// SetErrorMode function.</para>
        /// <para>LoadLibrary can be used to load a library module into
        /// the address space of the process and return a handle that can be used in
        /// GetProcAddress to get the address of a DLL function.
        /// LoadLibrary can also be used to load other executable
        /// modules. For example, the function can specify an .exe file to get a handle that can be used in
        /// FindResource or
        /// LoadResource. However, do not use
        /// LoadLibrary to run an .exe file. Instead, use
        /// the CreateProcess function.</para>
        /// <para>If the specified module is a DLL that is not already loaded for the calling process, the system calls the
        /// DLL's DllMain function with the
        /// DLL_PROCESS_ATTACH value. If
        /// DllMain returns TRUE,
        /// LoadLibrary returns a handle to the module. If
        /// DllMain returns FALSE,
        /// the system unloads the DLL from the process address space and
        /// LoadLibrary returns NULL. It is
        /// not safe to call LoadLibrary from
        /// DllMain. For more information, see the Remarks section in
        /// DllMain.</para>
        /// <para>Module handles are not global or inheritable. A call to
        /// LoadLibrary by one process does not produce a handle that
        /// another process can use — for example, in calling
        /// GetProcAddress. The other process must make its own
        /// call to LoadLibrary for the module before calling
        /// GetProcAddress.</para>
        /// <para>If lpFileName does not include a path and there is more than one loaded module with
        /// the same base name and extension, the function returns a handle to the module that was loaded first.</para>
        /// <para>If no file name extension is specified in the lpFileName parameter, the default
        /// library extension .dll is appended. However, the file name string can include a trailing point character (.) to
        /// indicate that the module name has no extension. When no path is specified, the function searches for loaded
        /// modules whose base name matches the base name of the module to be loaded. If the name matches, the load succeeds.
        /// Otherwise, the function searches for the file.</para>
        /// <para>The first directory searched is the directory containing the image file used to create the calling process
        /// (for more information, see the
        /// CreateProcess function). Doing this allows
        /// private dynamic-link library (DLL) files associated with a process to be found without adding the process's
        /// installed directory to the PATH environment variable. If a relative path is
        /// specified, the entire relative path is appended to every token in the DLL search path list. To load a module from
        /// a relative path without searching any other path, use
        /// GetFullPathName to get a nonrelative path and call
        /// LoadLibrary with the nonrelative path. For more
        /// information on the DLL search order, see
        /// Dynamic-Link Library Search Order.</para>
        /// <para>The search path can be altered using the
        /// SetDllDirectory function. This solution is recommended
        /// instead of using SetCurrentDirectory or
        /// hard-coding the full path to the DLL.</para>
        /// <para>If a path is specified and there is a redirection file for the application, the function searches for the
        /// module in the application's directory. If the module exists in the application's directory,
        /// LoadLibrary ignores the specified path and loads the
        /// module from the application's directory. If the module does not exist in the application's directory,
        /// LoadLibrary loads the module from the specified
        /// directory. For more information, see
        /// Dynamic Link Library Redirection.</para>
        /// <para>If you call LoadLibrary with the name of an assembly
        /// without a path specification and the assembly is listed in the system compatible manifest, the call is
        /// automatically redirected to the side-by-side assembly.</para>
        /// <para>The system maintains a per-process reference
        /// count on all loaded modules. Calling LoadLibrary
        /// increments the reference count. Calling the FreeLibrary or
        /// FreeLibraryAndExitThread function decrements
        /// the reference count. The system unloads a module when its reference count reaches zero or when the process
        /// terminates (regardless of the reference count).</para>
        /// <para>Windows Server 2003 and Windows XP:  The Visual C++ compiler supports a syntax that enables you to declare thread-local variables:
        /// _declspec(thread). If you use this syntax in a DLL, you will not be able to load the
        /// DLL explicitly using LoadLibrary on versions of Windows
        /// prior to Windows Vista. If your DLL will be loaded explicitly, you must use the thread local
        /// storage functions instead of _declspec(thread). For an example, see
        /// Using Thread Local Storage
        /// in a Dynamic Link Library.</para>Security Remarks<para>Do not use the SearchPath function to retrieve a path to
        /// a DLL for a subsequent LoadLibrary call. The
        /// SearchPath function uses a different search order than
        /// LoadLibrary and it does not use safe process search mode
        /// unless this is explicitly enabled by calling
        /// SetSearchPathMode with
        /// BASE_SEARCH_PATH_ENABLE_SAFE_SEARCHMODE. Therefore,
        /// SearchPath is likely to first search the user’s current
        /// working directory for the specified DLL. If an attacker has copied a malicious version of a DLL into the current
        /// working directory, the path retrieved by SearchPath will
        /// point to the malicious DLL, which LoadLibrary will then
        /// load.</para><para>Do not make assumptions about the operating system version based on a
        /// LoadLibrary call that searches for a DLL. If the
        /// application is running in an environment where the DLL is legitimately not present but a malicious version of
        /// the DLL is in the search path, the malicious version of the DLL may be loaded. Instead, use the recommended
        /// techniques described in
        /// Getting the System Version.</para>
        /// </remarks>
        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern IntPtr LoadLibrary(string lpFileName);
    }
}