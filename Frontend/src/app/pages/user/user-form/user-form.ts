import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../user';
import { User } from '../../../models/user.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-user-form',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './user-form.html',
  styleUrl: './user-form.css'
})
export class UserFormComponent implements OnInit {
  user: User = {};
  isEditMode: boolean = false;

  constructor(
    private userService: UserService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.userService.getUser(id).subscribe((user) => {
        this.user = user;
      });
    }
  }

  saveUser(): void {
    if (this.isEditMode && this.user.id) {
      this.userService.updateUser(this.user.id, this.user).subscribe(() => {
        this.router.navigate(['/users']);
      });
    } else {
      this.userService.addUser(this.user).subscribe(() => {
        this.router.navigate(['/users']);
      });
    }
  }

  goBack(): void {
    this.router.navigate(['/users']);
  }
}
