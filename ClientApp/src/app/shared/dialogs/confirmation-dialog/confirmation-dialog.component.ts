import { Component, OnInit, HostListener, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
  selector: 'app-confirmation-dialog',
  templateUrl: './confirmation-dialog.component.html',
  styleUrls: ['./confirmation-dialog.component.css']
})
export class ConfirmationDialogComponent implements OnInit {

  @HostListener('window:keyup.esc') onKeyUp() {
    let cn = confirm('Are you sure ?')
    if (cn) {
      this.dialogRef.close();
    }
  }

  @HostListener("window:beforeunload", ["$event"]) unloadHandler(event: Event) {
    console.log('event:', event);
    event.returnValue = false;
  }

  constructor(public dialogRef: MatDialogRef<ConfirmationDialogComponent>, @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit() {
    this.dialogRef.disableClose = true;
    this.dialogRef.backdropClick().subscribe(_ => {
      let cn = confirm('Sure ?')
      if (cn) {
        this.dialogRef.close(false);
      }
    });
  }

  onYesClick(): void {
    this.dialogRef.close(true);
  }

  public closeDialog = () => {
    this.dialogRef.close();
  }
}
