import { Routes, UrlSegment } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MembersListComponent } from './members/members-list/members-list.component';
import { MembersDetailComponent } from './members/members-detail/members-detail.component';
import { MessagesComponent } from './messages/messages.component';
import { ListsComponent } from './lists/lists.component';
import { authGuardGuard } from './guards/auth-guard.guard';
import { MembersEditComponent } from './members/members-edit/members-edit.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { unsavedStateGuard } from './guards/unsaved-state.guard';

export function stringOnlyMatcher(segments: UrlSegment[]) {
    if (segments.length === 2 && segments[0].path === 'members' && /^[a-zA-Z]+$/.test(segments[1].path)) {
        return {
            consumed: segments,
            posParams: {
                name: segments[1]
            }
        };
    }
    return null;
}

export const routes: Routes = [
    {
        path: '',
        component: HomeComponent,
        title: 'Home'
    },
    {
        path: '',
        canActivateChild: [authGuardGuard],
        runGuardsAndResolvers: 'always',
        children: [
            { path: 'members', component: MembersListComponent, title: 'Members' },
            { matcher: stringOnlyMatcher, component: MembersDetailComponent },
            { path: 'profile/edit', component: MembersEditComponent, title: 'Edit Member', canDeactivate: [unsavedStateGuard] },
            { path: 'lists', component: ListsComponent },
            { path: 'messages', component: MessagesComponent, title: 'Messages' },
        ]
    },
    { path: 'not-found', component: NotFoundComponent, title: 'Page Not Found' },
    { path: '**', redirectTo: 'not-found', pathMatch: 'full' }
];
