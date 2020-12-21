# Changelog

## [2.1.0-preview.1] - 2020-11-16
###Fixed
- Fixed null reference exception thrown on changing values of a material while recording animation (Case 1286034)

###Changed
- Minor version bump due to change in required Unity version

## [2.0.2-preview.1] - 2020-05-27
###Added
- Ability to change IK Gizmo colours
- Ability to hide individual IK Gizmo

###Changed
- IK Gizmo will not show for disabled IK Solvers.

## [2.0.1-preview.2] - 2020-04-01
### Fixed
- Fixed Null Reference Exception when restoring default pose for LimbSolver (Case 1223323)


## [2.0.0-preview.2] - 2020-02-10
### Fixed
- Fixed Null Reference Exception when restoring default pose when there is no IK Manager (Case 1211656)

## [2.0.0-preview.1] - 2019-08-24
### Changed
- Update to major version due to dependency major version upgrade

## [1.3.0-preview.2] - 2019-07-24
### Added
- Add related test packages

## [1.3.0-preview.1] - 2019-07-11
### Added
- Update dependency for Unity 2019.3 support

## [1.2.0-preview.1] - 2019-07-11
### Added
- Update dependency for Unity 2019.2 support

## [1.1.0-preview.1] - 2019-02-19
### Added
- Update dependency for Unity 2019.1 support

## [1.0.8-preview.1] - 2019-01-25
### Added
- Add compatibility for Unity 2019.1

## [1.0.7-preview.1] - 2018-11-15
### Added
- Fix IK SceneView Gizmo for Unity 2018.3
- Bumped minimum Unity version for this package to 2018.3

## [1.0.6-preview.1] - 2018-11-14
### Added
- Fix effector-target name. Effector becomes the transform that moves and the Target becomes the transform to reach.

## [1.0.6-preview] - 2018-06-20
### Added
- Fix compatibility issues with Bone Manipulator
- Restore default pose was not surviving between scenes.
- Fix case where parenting effectors and applying undo would make unity crash.
- Fix constrained target rotation in editor to SetLocalEulerHints
- Fix restore default pose and FK2IK should not rotate target when constrain rotation is false.
- Fix undo on dragging a bone in a chain

## [0.1.0] - 2017-11-20
### Added
- Edit Sprite Bind Pose
- Edit Sprite Skin Weights
- Edit Sprite Mesh
- IK
- Runtime Deformation using JobSystem