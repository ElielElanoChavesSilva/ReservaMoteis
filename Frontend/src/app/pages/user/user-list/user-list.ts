import { Component, OnInit } from '@angular/core';
import { UserService } from '../user';
import { User } from '../../../models/user.model';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [RouterLink, CommonModule],
  templateUrl: './user-list.html',
  styleUrl: './user-list.css',
})
export class UserListComponent implements OnInit {
  users: User[] = [];

  constructor(private userService: UserService, private router: Router) {}

  ngOnInit(): void {
    this.getUsers();
  }

  getUsers(): void {
    this.userService.getUsers().subscribe((users) => {
      this.users = users;
    });
  }

  deleteUser(id?: string): void {
    if (id) {
      this.userService.deleteUser(id).subscribe(() => {
        this.getUsers();
      });
    }
  }
}
