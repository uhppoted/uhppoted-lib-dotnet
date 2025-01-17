# TODO

- [ ] Package for NuGet
      - [x] "klingon is an invalid culture identifier."
      - [x] Copy README on build/release (generate ?)
      - [x] README: github registry install instructions
      - [x] README: fix documentation links
      - [x] Clean up integration test warning on terminate
      - [ ] Publish from github workflow
            - [x] nuget.org
            - [x] Github packages
            - [x] Check snupkg is uploaded
            - [x] Remove version extension
            - [ ] Automate file version
            ```
            echo ">>>>>> build Docker image version ${{ github.event.release.tag_name }}"
            ```

      - [ ] Test install from NuGet
      - [ ] Test install from Github packages

- [x] FAQ
      - https://support.microsoft.com/en-us/topic/you-cannot-exclude-ports-by-using-the-reservedports-registry-key-in-windows-server-2008-or-in-windows-server-2008-r2-a68373fd-9f64-4bde-9d68-c5eded74ea35
      - https://stackoverflow.com/questions/7006939/how-to-change-view-the-ephemeral-port-range-on-windows-machines#7007159
      - [You cannot exclude ports by using the ReservedPorts registry key in Windows Server 2008 or in Windows Server 2008 R2](https://support.microsoft.com/en-us/topic/you-cannot-exclude-ports-by-using-the-reservedports-registry-key-in-windows-server-2008-or-in-windows-server-2008-r2-a68373fd-9f64-4bde-9d68-c5eded74ea35)